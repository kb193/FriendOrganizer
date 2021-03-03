Imports System.ComponentModel.DataAnnotations
Imports System.Runtime.CompilerServices

Public Class ModelWrapper(Of T)
    Inherits NotifyDataErrorInfoBase

    Public Model As T

    Sub New(model As T)
        Me.Model = model
    End Sub

    Public Function GetValue(Of TValue)(<CallerMemberName> Optional propertyName As String = Nothing) As TValue
        Return DirectCast(GetType(T).GetProperty(propertyName).GetValue(Model), TValue)
    End Function

    Public Sub SetValue(Of TValue)(value As TValue, <CallerMemberName> Optional propertyName As String = Nothing)
        GetType(T).GetProperty(propertyName).SetValue(Model, value)
        OnPropertyChanged(propertyName)
        ValidatePropertyInternal(propertyName, value)
    End Sub

    Private Sub ValidatePropertyInternal(propName As String, value As Object)
        ClearErrors(propName)

        ValidateDataAnnotations(propName, value)

        ValidateCustomErrors(propName)
    End Sub

    Private Sub ValidateDataAnnotations(propName As String, value As Object)
        Dim context As New ValidationContext(Model) With {.MemberName = propName}
        Dim results As New List(Of ValidationResult)
        Validator.TryValidateProperty(value, context, results)

        For Each thing In results
            AddError(propName, thing.ErrorMessage)
        Next
    End Sub

    Private Sub ValidateCustomErrors(propName As String)
        Dim results = ValidateProperty(propName)
        If results IsNot Nothing Then
            For Each thing In results
                AddError(propName, thing)
            Next
        End If
    End Sub

    Protected Overridable Function ValidateProperty(propertyName As String) As IEnumerable(Of String)
        Return Nothing
    End Function
End Class
