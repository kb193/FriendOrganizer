Imports Model

Public Interface IMeetingLookupDataService
    Function GetMeetingLookupAsync() As Task(Of IEnumerable(Of LookupItem))
End Interface
