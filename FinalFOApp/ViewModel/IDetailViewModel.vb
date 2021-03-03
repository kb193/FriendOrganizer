Public Interface IDetailViewModel
    Function LoadAsync(Id As Integer) As Task

    Property HasChanges As Boolean
    Property Id As Integer
End Interface


