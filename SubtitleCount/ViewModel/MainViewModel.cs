using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microshaoft.Text;
using Microsoft.Practices.ServiceLocation;
using Ookii.Dialogs.Wpf;
using SubtitleCount.Models;

namespace SubtitleCount.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private string _status;
        private string _d;
        private ObservableCollection<SubtitleItem> _subtitles;

        private RelayCommand _addDirectoryCommand;
        private RelayCommand _addFileCommand;
        private RelayCommand _clearCommand;
        private RelayCommand _countCommand;
        private RelayCommand _outputCommand;

        private CalcParameter _selectedParameter;

        private Dictionary<string, CalcParameter> _types = new Dictionary<string, CalcParameter>
        {
            {"翻译", new CalcParameter(6, 1)},
            {"听写", new CalcParameter(0.6m, 1)},
            {"手抄", new CalcParameter(0.6m, 0.3m)},
            {"OCR", new CalcParameter(0, 0.3m)}
        };

        public MainViewModel()
        {
            this._subtitles = new ObservableCollection<SubtitleItem>();
            this.InitCommands();
        }

        private void InitCommands()
        {
            this._addFileCommand = new RelayCommand(() =>
            {
                var dialog = new VistaOpenFileDialog();
                dialog.Title = "Select one subtitle file to open";
                dialog.Filter = "ASS Files|*.ass|SRT Files|*.srt";
                dialog.Multiselect = false;
                dialog.CheckFileExists = true;
                dialog.ShowDialog();
                if (!string.IsNullOrWhiteSpace(dialog.FileName))
                {
                    this.AddSubtitle(dialog.FileName);
                }
            });

            this._addDirectoryCommand = new RelayCommand(() =>
            {
                var dialog = new VistaFolderBrowserDialog();
                dialog.ShowNewFolderButton = true;
                dialog.ShowDialog();
                if (!string.IsNullOrWhiteSpace(dialog.SelectedPath))
                {
                    var files = Directory.GetFiles(dialog.SelectedPath, "*.*", SearchOption.AllDirectories);
                    foreach (var file in files)
                    {
                        if (Path.HasExtension(file))
                        {
                            string ext = Path.GetExtension(file).ToUpperInvariant();
                            if (".SRT,.ASS".Split(',').Contains(ext))
                            {
                                this.AddSubtitle(file);
                            }
                        }
                    }
                }
            });

            this._clearCommand = new RelayCommand(() => this._subtitles.Clear());

            this._countCommand = new RelayCommand(() =>
            {
                var func = new Action<IList<SubtitleItem>>(Count);
                func.BeginInvoke(this._subtitles, CountCompleted, func);
            });

            this._outputCommand = new RelayCommand(this.Output);
        }

        private void AddSubtitle(string path)
        {
            if (!File.Exists(path))
            {
                return;
            }

            var subtitle = new SubtitleItem();
            subtitle.Name = Path.GetFileName(path);
            subtitle.Type = Path.GetExtension(path).ToUpperInvariant();
            subtitle.Path = path;

            this._subtitles.Add(subtitle);
        }

        private void Count(IList<SubtitleItem> subtitles)
        {
            this.UpdateState("统计中...");
            foreach (var subtitle in subtitles)
            {
                if (subtitle.Words == 0 && subtitle.Lines == 0)
                {
                    var count = ServiceLocator.Current.GetInstance<ISubtitleCount>(subtitle.Type);
                    var identify = new IdentifyEncoding();
                    string encodingName = identify.GetEncodingName(new FileInfo(subtitle.Path));
                    Encoding encoding = Encoding.GetEncoding(encodingName);
                    var result = count.Count(File.ReadAllText(subtitle.Path, encoding));
                    subtitle.Words = result.Words;
                    subtitle.Lines = result.Lines;
                }
            }
        }

        private void Output()
        {
            var builder = new StringBuilder();
            int maxTitleLength = this._subtitles.Max(c => GetLength(c.Name) + 10);

            builder.AppendLine("[pre]");
            builder.AppendFormat("{0} {1}   {2}   {3}   {4}   {5}", PadRightEx("字幕名称", maxTitleLength), "有效字数", "字数得分", "有效行数",
                "行数得分",
                "总得分");
            builder.AppendLine();
            decimal sum = 0;
            decimal d = decimal.TryParse(this._d, out d) ? d : 0;
            foreach (var subtitle in this._subtitles)
            {
                decimal itemSum = d * subtitle.Words * this._selectedParameter.A + d * subtitle.Lines * this._selectedParameter.B;
                sum += itemSum;

                builder.AppendFormat("{0} {1,-10} {2,-10} {3,-10} {4,-10} {5,-10}", PadRightEx(subtitle.Name, maxTitleLength),
                    subtitle.Words.ToString("0"), (d * subtitle.Words * this._selectedParameter.A).ToString("0.000"), subtitle.Lines.ToString("0"),
                    (d * subtitle.Lines * this._selectedParameter.B).ToString("0.000"),
                    itemSum.ToString("0.000"));
                builder.AppendLine();
            }
            builder.AppendFormat("D   ：{0}", d.ToString("0.000"));
            builder.AppendLine();
            builder.AppendFormat("总计：{0}", sum.ToString("0.000"));
            builder.AppendLine();
            builder.AppendLine("[/pre]");

            var output = new OutputWindow();
            output.Output.Text = builder.ToString();
            output.ShowDialog();
        }

        private static string PadRightEx(string str, int totalByteCount)
        {
            Encoding coding = Encoding.GetEncoding("gb2312");
            int dcount = 0;
            foreach (char ch in str.ToCharArray())
            {
                if (coding.GetByteCount(ch.ToString()) == 2)
                    dcount++;
            }
            string w = str.PadRight(totalByteCount - dcount);
            return w;
        }

        private int GetLength(string str)
        {
            int length = str.Length;
            Encoding coding = Encoding.GetEncoding("gb2312");
            foreach (char ch in str.ToCharArray())
            {
                if (coding.GetByteCount(ch.ToString()) == 2)
                    length++;
            }
            return length;
        }

        private void CountCompleted(IAsyncResult ir)
        {
            var func = ir.AsyncState as Action<IList<SubtitleItem>>;
            func.EndInvoke(ir);
            this.UpdateState("统计完成。");
        }

        private void UpdateState(string content)
        {
            this.Set(() => this.Status, ref this._status, content);
        }

        public ObservableCollection<SubtitleItem> Subtitles
        {
            get { return _subtitles; }
            set { _subtitles = value; }
        }

        public RelayCommand AddDirectoryCommand
        {
            get { return _addDirectoryCommand; }
            set { _addDirectoryCommand = value; }
        }

        public RelayCommand AddFileCommand
        {
            get { return _addFileCommand; }
            set { _addFileCommand = value; }
        }

        public RelayCommand ClearCommand
        {
            get { return _clearCommand; }
            set { _clearCommand = value; }
        }

        public RelayCommand CountCommand
        {
            get { return _countCommand; }
            set { _countCommand = value; }
        }

        public RelayCommand OutputCommand
        {
            get { return _outputCommand; }
            set { _outputCommand = value; }
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public Dictionary<string, CalcParameter> Types
        {
            get { return _types; }
            set { _types = value; }
        }

        public CalcParameter SelectedParameter
        {
            get { return _selectedParameter; }
            set { _selectedParameter = value; }
        }

        public string D
        {
            get { return _d; }
            set { _d = value; }
        }
    }
}