Imports DataAccess
Imports Model
Imports System.Data.Entity

Public Class FriendRepository
    Inherits GenericRepository(Of [Friend], FinalDbContext)
    Implements IFriendRepository

    Public Sub New(context As FinalDbContext)
        MyBase.New(context)
    End Sub

    Public Sub RemovePhoneNumber(number As FriendPhoneNumber) Implements IFriendRepository.RemovePhoneNumber
        Context.PhoneNumbers.Remove(number)
    End Sub

    Public Overrides Async Function GetByIdAsync(id As Integer) As Task(Of [Friend])
        Return Await Context.Friends.Include(Function(i) i.PhoneNumbers).SingleAsync(Function(i) i.Id = id)
    End Function

    Public Async Function HasMeetingsAsync(friendId As Integer) As Task(Of Boolean) Implements IFriendRepository.HasMeetingsAsync
        Return Await Context.Meetings.AsNoTracking.Include(Function(i) i.Friends).AnyAsync(Function(j) j.Friends.Any(Function(k) k.Id = friendId))
    End Function
End Class
