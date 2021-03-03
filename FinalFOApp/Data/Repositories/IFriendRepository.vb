Imports Model

Public Interface IFriendRepository
    Inherits IGenericRepository(Of [Friend])
    Sub RemovePhoneNumber(number As FriendPhoneNumber)
    Function HasMeetingsAsync(friendId As Integer) As Task(Of Boolean)
End Interface
