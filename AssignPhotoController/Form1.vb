﻿Imports System.Xml


Public Class Form1

    Friend Shared Notify As System.Windows.Forms.NotifyIcon

    Private Sub StartService()
        If Not ServiceController1.Status = 4 Then ServiceController1.Start()
        SetMenu()
    End Sub

    Private Sub StopService()
        If Not ServiceController1.Status = 1 Then ServiceController1.Stop()
        SetMenu()
    End Sub

    Private Sub RestartService()
        StopService()
        ServiceController1.WaitForStatus(1)
        StartService()
    End Sub

    Private Sub SetMenu()

        mnuStart.Enabled = False
        mnuStop.Enabled = False


        ServiceController1.Refresh()

CHECK_AGAIN:

        Select Case ServiceController1.Status

            Case 1
                mnuStart.Enabled = True
            Case 4
                mnuStop.Enabled = True
            Case Else
                System.Threading.Thread.Sleep(2000)
                ServiceController1.Refresh()
                GoTo CHECK_AGAIN

        End Select

    End Sub



    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Notify = niAssignPhoto
        Me.niAssignPhoto.ShowBalloonTip(5000)

        StartService()

        SetMenu()

    End Sub

    Private Sub mnuStop_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuStop.Click
        StopService()
    End Sub

    Private Sub mnuStart_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuStart.Click
        StartService()
    End Sub

    Private Sub mnuExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuExit.Click
        StopService()
        niAssignPhoto.Visible = False
        niAssignPhoto.Dispose()
        End
    End Sub

    Private Sub LoadSettings()
        Dim XmlDoc As XmlDocument = New XmlDocument()

        XmlDoc.Load(My.Application.Info.DirectoryPath & "\AssignPhotoService.exe.config")

        For Each xElement As XmlElement In XmlDoc.DocumentElement
            If xElement.Name = "userSettings" Then
                For Each xNode As XmlNode In xElement.ChildNodes
                    For Each node As XmlNode In xNode.ChildNodes
                        If node.Attributes(0).InnerText = "DataSource" Then txtServer.Text = node.InnerText
                        If node.Attributes(0).InnerText = "DatabaseName" Then txtDatabase.Text = node.InnerText
                        If node.Attributes(0).InnerText = "UserName" Then txtUserName.Text = node.InnerText
                        If node.Attributes(0).InnerText = "Password" Then txtPassword.Text = node.InnerText
                        If node.Attributes(0).InnerText = "ActionTime" Then dtTime.Text = node.InnerText
                        If node.Attributes(0).InnerText = "ActionInterval" Then txtInterval.Text = node.InnerText
                        If node.Attributes(0).InnerText = "SubmittedFolder" Then txtSubmitted.Text = node.InnerText
                        If node.Attributes(0).InnerText = "UpdatedFolder" Then txtUpdated.Text = node.InnerText
                        If node.Attributes(0).InnerText = "RejectedFolder" Then txtRejected.Text = node.InnerText
                    Next
                Next
            End If
        Next

    End Sub
    Private Sub UpdateSettings()

        Dim XmlDoc As XmlDocument = New XmlDocument()

        XmlDoc.Load(My.Application.Info.DirectoryPath & "\AssignPhotoService.exe.config")

        For Each xElement As XmlElement In XmlDoc.DocumentElement
            If xElement.Name = "userSettings" Then
                For Each xNode As XmlNode In xElement.ChildNodes
                    For Each node As XmlNode In xNode.ChildNodes
                        If node.Attributes(0).InnerXml = "DataSource" Then node.FirstChild.InnerXml = txtServer.Text
                        If node.Attributes(0).InnerText = "DatabaseName" Then node.FirstChild.InnerXml = txtDatabase.Text
                        If node.Attributes(0).InnerText = "UserName" Then node.FirstChild.InnerXml = txtUserName.Text
                        If node.Attributes(0).InnerText = "Password" Then node.FirstChild.InnerXml = txtPassword.Text
                        If node.Attributes(0).InnerText = "ActionTime" Then node.FirstChild.InnerXml = dtTime.Text
                        If node.Attributes(0).InnerText = "ActionInterval" Then node.FirstChild.InnerXml = txtInterval.Text
                        If node.Attributes(0).InnerText = "SubmittedFolder" Then node.FirstChild.InnerXml = txtSubmitted.Text
                        If node.Attributes(0).InnerText = "UpdatedFolder" Then node.FirstChild.InnerXml = txtUpdated.Text
                        If node.Attributes(0).InnerText = "RejectedFolder" Then node.FirstChild.InnerXml = txtRejected.Text
                    Next
                Next
            End If
        Next

        XmlDoc.Save(My.Application.Info.DirectoryPath & "\AssignPhotoService.exe.config")

    End Sub

    Private Sub mnuSettings_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuSettings.Click
        Me.Show()
        Me.Width = 290
        Me.Height = 290
        Me.WindowState = FormWindowState.Normal
        LoadSettings()
        txtServer.Focus()
    End Sub

    Private Sub btnApply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApply.Click
        UpdateSettings()
        Me.WindowState = FormWindowState.Minimized
        RestartService()
        Me.Hide()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.WindowState = FormWindowState.Minimized
        Me.Hide()
    End Sub

    Private Sub btnSubmitted_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSubmitted.Click
        If Not FolderBrowserDialog1.ShowDialog = Windows.Forms.DialogResult.Cancel Then
            txtSubmitted.Text = FolderBrowserDialog1.SelectedPath & "\"
        End If
    End Sub

    Private Sub btnUpdated_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdated.Click
        If Not FolderBrowserDialog1.ShowDialog = Windows.Forms.DialogResult.Cancel Then
            txtUpdated.Text = FolderBrowserDialog1.SelectedPath & "\"
        End If
    End Sub

    Private Sub btnRejected_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRejected.Click
        If Not FolderBrowserDialog1.ShowDialog = Windows.Forms.DialogResult.Cancel Then
            txtRejected.Text = FolderBrowserDialog1.SelectedPath & "\"
        End If
    End Sub

    Private Sub Form1_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        Me.Width = 412
        Me.Height = 412
    End Sub
End Class
