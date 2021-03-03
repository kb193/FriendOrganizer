Imports System.ComponentModel.DataAnnotations

Public Class ProgrammingLanguage

    Public Property Id As Integer

    <Required>
    <MaxLength(50)>
    Public Property Name As String

End Class
