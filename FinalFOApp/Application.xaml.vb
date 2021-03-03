Imports Autofac

Class Application
    Private Sub Application_Startup(sender As Object, e As StartupEventArgs)
        Dim bootstrapper As New Bootstrapper
        Dim container = bootstrapper.Bootstrap

        Dim mainWindow = container.Resolve(Of MainWindow)
        mainWindow.Show()
    End Sub

    ' Application-level events, such as Startup, Exit, and DispatcherUnhandledException
    ' can be handled in this file.

End Class
