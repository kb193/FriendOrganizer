Imports DataAccess
Imports Model
Imports System.Data.Entity
Public Class MeetingRepository
    Inherits GenericRepository(Of Meeting, FinalDbContext)
    Implements IMeetingRepository

    Public Sub New(context As FinalDbContext)
        MyBase.New(context)
    End Sub

    Public Overrides Async Function GetByIdAsync(id As Integer) As Task(Of Meeting) Implements IMeetingRepository.GetByIdAsync
        Return Await Context.Meetings.Include(Function(i) i.Friends).SingleAsync(Function(j) j.Id = id)
    End Function

    Public Async Function GetAllFriendsAsync() As Task(Of IEnumerable(Of [Friend])) Implements IMeetingRepository.GetAllFriendsAsync
        Return Await Context.Friends.ToListAsync
    End Function

    Public Async Function ReloadFriendAsync(id As Integer) As Task Implements IMeetingRepository.ReloadFriendAsync
        Dim dbEntityEntry = Context.ChangeTracker.Entries(Of [Friend]).SingleOrDefault(Function(i) i.Entity.Id = id)
        If dbEntityEntry IsNot Nothing Then
            Await dbEntityEntry.ReloadAsync
        End If
    End Function

End Class
