Imports Model

Public Interface IFriendLookupDataService
    Function GetFriendLookupAsync() As Task(Of IEnumerable(Of LookupItem))
End Interface
