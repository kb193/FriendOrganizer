Imports Model

Public Class ProgrammingLanguageWrapper
    Inherits ModelWrapper(Of ProgrammingLanguage)

    Public Sub New(model As ProgrammingLanguage)
        MyBase.New(model)
    End Sub

    Public ReadOnly Property Id As Integer
        Get
            Return Model.Id
        End Get
    End Property

    Public Property Name As String
        Get
            Return GetValue(Of String)()
        End Get
        Set(value As String)
            SetValue(value)
        End Set
    End Property
End Class
