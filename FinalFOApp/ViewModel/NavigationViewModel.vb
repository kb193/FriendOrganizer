Imports System.Collections.ObjectModel
Imports Model
Imports Prism.Events

Public Class NavigationViewModel
    Inherits ViewModelBase
    Implements INavigationViewModel

    Private _friendLookupDataService As IFriendLookupDataService
    Private _meetingLookupDataService As IMeetingLookupDataService
    Private _eventAggregator As IEventAggregator

    Public Property Friends As New ObservableCollection(Of NavigationItemViewModel)
    Public Property Meetings As New ObservableCollection(Of NavigationItemViewModel)

    Sub New(eventAggregator As IEventAggregator,
            friendLookupService As IFriendLookupDataService,
            meetingLookupService As IMeetingLookupDataService)
        Me._eventAggregator = eventAggregator
        Me._friendLookupDataService = friendLookupService
        Me._meetingLookupDataService = meetingLookupService

        _eventAggregator.GetEvent(Of AfterDetailSavedEvent).Subscribe(AddressOf AfterDetailSaved)
        _eventAggregator.GetEvent(Of AfterDetailDeletedEvent).Subscribe(AddressOf AfterDetailDeleted)
    End Sub

    Public Async Function LoadAsync() As Task Implements INavigationViewModel.LoadAsync
        Friends.Clear()

        Dim friendsLookup = Await _friendLookupDataService.GetFriendLookupAsync
        For Each fren In friendsLookup
            Friends.Add(New NavigationItemViewModel(fren.Id, fren.DisplayMember, _eventAggregator, NameOf(FriendDetailViewModel)))
        Next

        Meetings.Clear()

        Dim meetingsLookup = Await _meetingLookupDataService.GetMeetingLookupAsync
        For Each meet In meetingsLookup
            Meetings.Add(New NavigationItemViewModel(meet.Id, meet.DisplayMember, _eventAggregator, NameOf(MeetingDetailViewModel)))
        Next
    End Function

    Private Sub AfterDetailSaved(args As AfterDetailSavedEventArgs)
        Select Case args.ViewModelName
            Case NameOf(FriendDetailViewModel)
                AfterDetailSaved(Friends, args)
            Case NameOf(MeetingDetailViewModel)
                AfterDetailSaved(Meetings, args)
        End Select
    End Sub

    Private Sub AfterDetailSaved(items As ObservableCollection(Of NavigationItemViewModel), args As AfterDetailSavedEventArgs)
        Dim lookupItem = items.SingleOrDefault(Function(i) i.Id = args.Id)

        If lookupItem Is Nothing Then
            items.Add(New NavigationItemViewModel(args.Id, args.DisplayMember, _eventAggregator, args.ViewModelName))
        Else
            lookupItem.DisplayMember = args.DisplayMember
        End If
    End Sub


    Private Sub AfterDetailDeleted(args As AfterDetailDeletedEventArgs)
        Select Case args.ViewModelName
            Case NameOf(FriendDetailViewModel)
                AfterDetailDeleted(Friends, args)
            Case NameOf(MeetingDetailViewModel)
                AfterDetailDeleted(Meetings, args)
        End Select
    End Sub

    Private Sub AfterDetailDeleted(items As ObservableCollection(Of NavigationItemViewModel), args As AfterDetailDeletedEventArgs)
        Dim item = items.SingleOrDefault(Function(i) i.Id = args.Id)

        If items IsNot Nothing Then
            items.Remove(item)
        End If
    End Sub
End Class
