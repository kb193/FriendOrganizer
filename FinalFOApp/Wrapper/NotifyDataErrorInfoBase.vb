Imports System.ComponentModel

Public Class NotifyDataErrorInfoBase
    Inherits ViewModelBase
    Implements INotifyDataErrorInfo

    Public PropertyErrors As New Dictionary(Of String, List(Of String))

    Public ReadOnly Property HasErrors As Boolean Implements INotifyDataErrorInfo.HasErrors
        Get
            Return PropertyErrors.Any
        End Get
    End Property

    Public Event ErrorsChanged As EventHandler(Of DataErrorsChangedEventArgs) Implements INotifyDataErrorInfo.ErrorsChanged

    Public Function GetErrors(propertyName As String) As IEnumerable Implements INotifyDataErrorInfo.GetErrors
        If PropertyErrors.ContainsKey(propertyName) Then
            Return PropertyErrors(propertyName)
        End If
        Return Nothing
    End Function

    Public Sub AddError(propertyName As String, err As String)
        If PropertyErrors.ContainsKey(propertyName) = False Then
            PropertyErrors(propertyName) = New List(Of String)
        End If
        If PropertyErrors(propertyName).Contains(err) = False Then
            PropertyErrors(propertyName).Add(err)
            OnErrorsChanged(propertyName)
        End If
    End Sub

    Public Overridable Sub OnErrorsChanged(propertyName As String)
        RaiseEvent ErrorsChanged(Me, New DataErrorsChangedEventArgs(propertyName))
        OnPropertyChanged(NameOf(HasErrors))
    End Sub

    Public Sub ClearErrors(propName As String)
        If PropertyErrors.ContainsKey(propName) Then
            PropertyErrors.Remove(propName)
            OnErrorsChanged(propName)
        End If
    End Sub
End Class
