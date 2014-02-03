using Autofac;
using Autofac.Extras.CommonServiceLocator;
using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;

namespace SubtitleCount.ViewModel
{
    public class ViewModelLocator
    {

        public ViewModelLocator()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<MainViewModel>();
            builder.RegisterType<ASSCount>().Named<ISubtitleCount>(".ASS");
            builder.RegisterType<SRTCount>().Named<ISubtitleCount>(".SRT");

            var container = builder.Build();

            var locator = new AutofacServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => locator);
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}