Imports System.Collections.ObjectModel
Imports Autofac.Features.Indexed
Imports Prism.Commands
Imports Prism.Events

Public Class MainWindowViewModel
    Inherits ViewModelBase

    Private _detailViewModelCreator As IIndex(Of String, IDetailViewModel)
    Private _selectedDetailViewModel As IDetailViewModel
    Private _eventAggregator As IEventAggregator

    Public Property CreateNewDetailCommand As ICommand
    Public Property OpenSingleDetailViewCommand As ICommand

    Public Property DetailViewModels As New ObservableCollection(Of IDetailViewModel)
    Public Property NavigationViewModel As INavigationViewModel
    Private _messageDialogService As IMessageDialogService

    Public Property SelectedDetailViewModel As IDetailViewModel
        Get
            Return _selectedDetailViewModel
        End Get
        Set(value As IDetailViewModel)
            _selectedDetailViewModel = value
            OnPropertyChanged()
        End Set
    End Property

    Sub New(detailViewModelCreator As IIndex(Of String, IDetailViewModel),
            navigationViewModel As INavigationViewModel,
            eventAggregator As IEventAggregator,
            messageDialogService As IMessageDialogService)

        Me._eventAggregator = eventAggregator
        Me._detailViewModelCreator = detailViewModelCreator
        Me.NavigationViewModel = navigationViewModel
        Me._messageDialogService = messageDialogService

        CreateNewDetailCommand = New DelegateCommand(Of Type)(AddressOf OnCreateNewDetailExecute)
        OpenSingleDetailViewCommand = New DelegateCommand(Of Type)(AddressOf OnOpenSingleDetailViewExecute)

        _eventAggregator.GetEvent(Of OpenDetailViewEvent).Subscribe(AddressOf OnOpenDetailView)
        _eventAggregator.GetEvent(Of AfterDetailDeletedEvent).Subscribe(AddressOf OnAfterDetailDeleted)
        _eventAggregator.GetEvent(Of AfterDetailClosedEvent).Subscribe(AddressOf OnAfterDetailClosed)
    End Sub

    Public Async Function LoadAsync() As Task
        Await NavigationViewModel.LoadAsync
    End Function

    Private Sub RemoveDetailViewModel(id As Integer, viewModelName As String)
        Dim vm = DetailViewModels.SingleOrDefault(Function(i) i.Id = id And i.GetType.Name = viewModelName)

        If vm IsNot Nothing Then
            DetailViewModels.Remove(vm)
        End If
    End Sub

    Private Sub OnAfterDetailDeleted(args As AfterDetailDeletedEventArgs)
        RemoveDetailViewModel(args.Id, args.ViewModelName)
    End Sub

    Private Sub OnAfterDetailClosed(args As AfterDetailClosedEventArgs)
        RemoveDetailViewModel(args.Id, args.ViewModelName)
    End Sub

    Private Sub OnOpenSingleDetailViewExecute(viewModel As Type)
        OnOpenDetailView(New OpenDetailViewEventArgs With {.Id = -1, .ViewModelName = viewModel.Name})
    End Sub

    Private Async Sub OnOpenDetailView(args As OpenDetailViewEventArgs)
        Dim vm = DetailViewModels.SingleOrDefault(Function(i) i.Id = args.Id And i.GetType.Name = args.ViewModelName)

        If vm Is Nothing Then
            vm = _detailViewModelCreator(args.ViewModelName)
            Await vm.LoadAsync(args.Id)
            DetailViewModels.Add(vm)
        End If

        SelectedDetailViewModel = vm
    End Sub

    Dim nextNewItemId As Integer = 0
    Private Sub OnCreateNewDetailExecute(ViewModelType As Type)
        OnOpenDetailView(New OpenDetailViewEventArgs With {
                         .Id = nextNewItemId,
                         .ViewModelName = ViewModelType.Name})
        nextNewItemId -= 1
    End Sub
End Class
