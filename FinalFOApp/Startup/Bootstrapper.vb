Imports Autofac
Imports Autofac.Core
Imports DataAccess
Imports Prism.Events

Public Class Bootstrapper

    Public Function Bootstrap() As IContainer
        Dim builder As New ContainerBuilder

        builder.RegisterType(Of EventAggregator).As(Of IEventAggregator).SingleInstance()
        builder.RegisterType(Of FinalDbContext).AsSelf
        builder.RegisterType(Of MainWindow).AsSelf
        builder.RegisterType(Of MessageDialogService).As(Of IMessageDialogService)()

        builder.RegisterType(Of MainWindowViewModel).AsSelf
        builder.RegisterType(Of NavigationViewModel).As(Of INavigationViewModel)()
        builder.RegisterType(Of FriendDetailViewModel).Keyed(Of IDetailViewModel)(NameOf(FriendDetailViewModel))
        builder.RegisterType(Of MeetingDetailViewModel).Keyed(Of IDetailViewModel)(NameOf(MeetingDetailViewModel))
        builder.RegisterType(Of ProgrammingLanguageDetailViewModel).Keyed(Of IDetailViewModel)(NameOf(ProgrammingLanguageDetailViewModel))

        builder.RegisterType(Of LookupDataService).AsImplementedInterfaces
        builder.RegisterType(Of FriendRepository).As(Of IFriendRepository)()
        builder.RegisterType(Of MeetingRepository).As(Of IMeetingRepository)()
        builder.RegisterType(Of ProgrammingLanguageRepository).As(Of IProgrammingLanguageRepository)()
        Return builder.Build
    End Function

End Class
