Imports Model

Public Interface IMeetingRepository
    Inherits IGenericRepository(Of Meeting)

    Function GetAllFriendsAsync() As Task(Of IEnumerable(Of [Friend]))
    Function ReloadFriendAsync(id As Integer) As Task
End Interface
