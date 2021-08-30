Imports System.Runtime.InteropServices

' SHGetFileInfo関数で使用する構造体
Public Structure SHFILEINFO
    Public hIcon As IntPtr
    Public iIcon As IntPtr
    Public dwAttributes As Integer
    <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=260)> _
    Public szDisplayName As String
    <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=80)> _
    Public szTypeName As String
End Structure

Public Module functions
    ' SHGetFileInfo関数
    Private Declare Ansi Function SHGetFileInfo Lib "shell32.dll" (ByVal pszPath As String, ByVal dwFileAttributes As Integer, ByRef psfi As SHFILEINFO, ByVal cbFileInfo As Integer, ByVal uFlags As Integer) As IntPtr

    ' SHGetFileInfo関数で使用するフラグ
    Const SHGFI_ICON As Integer = &H100 ' アイコン・リソースの取得
    Const SHGFI_LARGEICON As Integer = &H0 ' 大きいアイコン
    Const SHGFI_SMALLICON As Integer = &H1 ' 小さいアイコン

    Public Sub openAllApp()
        Dim isDoubleRun(30) As Boolean  '0~30の31個
        For i As Integer = 0 To 30
            isDoubleRun(i) = False
        Next

        '全体のディレイ
        System.Threading.Thread.Sleep(My.Settings.nudAllDelayTime * 1000)

        'If My.Settings.chbDoubleRun = True Then
        'Dim ps As System.Diagnostics.Process() _
        '= System.Diagnostics.Process.GetProcesses()

        'Dim mc As New System.Management.ManagementClass("Win32_Process")
        'Dim moc As System.Management.ManagementObjectCollection = mc.GetInstances()
        'Dim mo As System.Management.ManagementObject

        'For Each mo In moc
        '    Try
        '        Dim i As Integer = 0

        'Dim strInputExeFile As String _
        '        = System.IO.Path.GetFileName(My.Settings.filePath1)

        'Dim strInputExeFile As String = My.Settings.filePath1
        'Dim strRunExeFile As String = mo("ExecutablePath")

        'MessageBox.Show(strInputExeFile & vbCrLf & strRunExeFile, "1行目が入力パス、2行目が起動中パス", MessageBoxButtons.OK)

        'If (strRunExeFile.ToLower()).Contains("main.txt") Then
        '    isDoubleRun(i) = True
        '    MessageBox.Show(strInputExeFile & vbCrLf & strRunExeFile, "二重起動確認できました", MessageBoxButtons.OK)
        '    'Exit For
        'End If
        'i += 1
        '        mo.Dispose()
        '    Catch ex As Exception
        'なにもしない（なぜかは不明だが一致するものがないとここになる）
        'End Try
        'Next
        'moc.Dispose()
        'mc.Dispose()
        'MessageBox.Show("Success！", "3", MessageBoxButtons.OK)
        'End If

        Dim blnError As Boolean = False
        Try
            If (My.Settings.disabled1 <> True) And (My.Settings.filePath1 <> Nothing) And ((isDoubleRun(0) = False And My.Settings.chbDoubleRun = True) Or My.Settings.chbDoubleRun = False) Then
                Dim r As New System.Text.RegularExpressions.Regex(".* (?<args>" & ControlChars.Quote & ".*\" & ControlChars.Quote & ")$", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                Dim m As System.Text.RegularExpressions.Match = r.Match(My.Settings.filePath1)
                Dim args As String = m.Groups("args").Value
                Dim startinfo = New ProcessStartInfo(My.Settings.filePath1)
                If args <> "" Then
                    startinfo.FileName = My.Settings.filePath1.Replace(" " & args, "")
                End If
                startinfo.Arguments = args.Replace(ControlChars.Quote, "")

                startinfo.UseShellExecute = True
                If My.Settings.windowState1 = 1 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Minimized
                ElseIf My.Settings.windowState1 = 2 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Maximized
                End If

                Process.Start(startinfo)
                System.Threading.Thread.Sleep(My.Settings.fileDelay1 * 1000)
            End If
        Catch ex As System.ComponentModel.Win32Exception
            blnError = True
        End Try
        Try
            If (My.Settings.disabled2 <> True) And (My.Settings.filePath2 <> Nothing) And ((isDoubleRun(0) = False And My.Settings.chbDoubleRun = True) Or My.Settings.chbDoubleRun = False) Then
                Dim r As New System.Text.RegularExpressions.Regex(".* (?<args>" & ControlChars.Quote & ".*\" & ControlChars.Quote & ")$", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                Dim m As System.Text.RegularExpressions.Match = r.Match(My.Settings.filePath2)
                Dim args As String = m.Groups("args").Value
                Dim startinfo = New ProcessStartInfo(My.Settings.filePath2)
                If args <> "" Then
                    startinfo.FileName = My.Settings.filePath2.Replace(" " & args, "")
                End If
                startinfo.Arguments = args.Replace(ControlChars.Quote, "")

                startinfo.UseShellExecute = True
                If My.Settings.windowState2 = 1 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Minimized
                ElseIf My.Settings.windowState2 = 2 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Maximized
                End If

                Process.Start(startinfo)
                System.Threading.Thread.Sleep(My.Settings.fileDelay2 * 1000)
            End If
        Catch ex As System.ComponentModel.Win32Exception
            blnError = True
        End Try
        Try
            If (My.Settings.disabled3 <> True) And (My.Settings.filePath3 <> Nothing) And ((isDoubleRun(0) = False And My.Settings.chbDoubleRun = True) Or My.Settings.chbDoubleRun = False) Then
                Dim r As New System.Text.RegularExpressions.Regex(".* (?<args>" & ControlChars.Quote & ".*\" & ControlChars.Quote & ")$", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                Dim m As System.Text.RegularExpressions.Match = r.Match(My.Settings.filePath3)
                Dim args As String = m.Groups("args").Value
                Dim startinfo = New ProcessStartInfo(My.Settings.filePath3)
                If args <> "" Then
                    startinfo.FileName = My.Settings.filePath3.Replace(" " & args, "")
                End If
                startinfo.Arguments = args.Replace(ControlChars.Quote, "")

                startinfo.UseShellExecute = True
                If My.Settings.windowState3 = 1 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Minimized
                ElseIf My.Settings.windowState3 = 2 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Maximized
                End If

                Process.Start(startinfo)
                System.Threading.Thread.Sleep(My.Settings.fileDelay3 * 1000)
            End If
        Catch ex As System.ComponentModel.Win32Exception
            blnError = True
        End Try
        Try
            If (My.Settings.disabled4 <> True) And (My.Settings.filePath4 <> Nothing) And ((isDoubleRun(0) = False And My.Settings.chbDoubleRun = True) Or My.Settings.chbDoubleRun = False) Then
                Dim r As New System.Text.RegularExpressions.Regex(".* (?<args>" & ControlChars.Quote & ".*\" & ControlChars.Quote & ")$", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                Dim m As System.Text.RegularExpressions.Match = r.Match(My.Settings.filePath4)
                Dim args As String = m.Groups("args").Value
                Dim startinfo = New ProcessStartInfo(My.Settings.filePath4)
                If args <> "" Then
                    startinfo.FileName = My.Settings.filePath4.Replace(" " & args, "")
                End If
                startinfo.Arguments = args.Replace(ControlChars.Quote, "")

                startinfo.UseShellExecute = True
                If My.Settings.windowState4 = 1 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Minimized
                ElseIf My.Settings.windowState4 = 2 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Maximized
                End If

                Process.Start(startinfo)
                System.Threading.Thread.Sleep(My.Settings.fileDelay4 * 1000)
            End If
        Catch ex As System.ComponentModel.Win32Exception
            blnError = True
        End Try
        Try
            If (My.Settings.disabled5 <> True) And (My.Settings.filePath5 <> Nothing) And ((isDoubleRun(0) = False And My.Settings.chbDoubleRun = True) Or My.Settings.chbDoubleRun = False) Then
                Dim r As New System.Text.RegularExpressions.Regex(".* (?<args>" & ControlChars.Quote & ".*\" & ControlChars.Quote & ")$", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                Dim m As System.Text.RegularExpressions.Match = r.Match(My.Settings.filePath5)
                Dim args As String = m.Groups("args").Value
                Dim startinfo = New ProcessStartInfo(My.Settings.filePath5)
                If args <> "" Then
                    startinfo.FileName = My.Settings.filePath5.Replace(" " & args, "")
                End If
                startinfo.Arguments = args.Replace(ControlChars.Quote, "")

                startinfo.UseShellExecute = True
                If My.Settings.windowState5 = 1 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Minimized
                ElseIf My.Settings.windowState5 = 2 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Maximized
                End If

                Process.Start(startinfo)
                System.Threading.Thread.Sleep(My.Settings.fileDelay5 * 1000)
            End If
        Catch ex As System.ComponentModel.Win32Exception
            blnError = True
        End Try
        Try
            If (My.Settings.disabled6 <> True) And (My.Settings.filePath6 <> Nothing) And ((isDoubleRun(0) = False And My.Settings.chbDoubleRun = True) Or My.Settings.chbDoubleRun = False) Then
                Dim r As New System.Text.RegularExpressions.Regex(".* (?<args>" & ControlChars.Quote & ".*\" & ControlChars.Quote & ")$", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                Dim m As System.Text.RegularExpressions.Match = r.Match(My.Settings.filePath6)
                Dim args As String = m.Groups("args").Value
                Dim startinfo = New ProcessStartInfo(My.Settings.filePath6)
                If args <> "" Then
                    startinfo.FileName = My.Settings.filePath6.Replace(" " & args, "")
                End If
                startinfo.Arguments = args.Replace(ControlChars.Quote, "")

                startinfo.UseShellExecute = True
                If My.Settings.windowState6 = 1 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Minimized
                ElseIf My.Settings.windowState6 = 2 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Maximized
                End If

                Process.Start(startinfo)
                System.Threading.Thread.Sleep(My.Settings.fileDelay6 * 1000)
            End If
        Catch ex As System.ComponentModel.Win32Exception
            blnError = True
        End Try
        Try
            If (My.Settings.disabled7 <> True) And (My.Settings.filePath7 <> Nothing) And ((isDoubleRun(0) = False And My.Settings.chbDoubleRun = True) Or My.Settings.chbDoubleRun = False) Then
                Dim r As New System.Text.RegularExpressions.Regex(".* (?<args>" & ControlChars.Quote & ".*\" & ControlChars.Quote & ")$", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                Dim m As System.Text.RegularExpressions.Match = r.Match(My.Settings.filePath7)
                Dim args As String = m.Groups("args").Value
                Dim startinfo = New ProcessStartInfo(My.Settings.filePath7)
                If args <> "" Then
                    startinfo.FileName = My.Settings.filePath7.Replace(" " & args, "")
                End If
                startinfo.Arguments = args.Replace(ControlChars.Quote, "")

                startinfo.UseShellExecute = True
                If My.Settings.windowState7 = 1 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Minimized
                ElseIf My.Settings.windowState7 = 2 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Maximized
                End If

                Process.Start(startinfo)
                System.Threading.Thread.Sleep(My.Settings.fileDelay7 * 1000)
            End If
        Catch ex As System.ComponentModel.Win32Exception
            blnError = True
        End Try
        Try
            If (My.Settings.disabled8 <> True) And (My.Settings.filePath8 <> Nothing) And ((isDoubleRun(0) = False And My.Settings.chbDoubleRun = True) Or My.Settings.chbDoubleRun = False) Then
                Dim r As New System.Text.RegularExpressions.Regex(".* (?<args>" & ControlChars.Quote & ".*\" & ControlChars.Quote & ")$", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                Dim m As System.Text.RegularExpressions.Match = r.Match(My.Settings.filePath8)
                Dim args As String = m.Groups("args").Value
                Dim startinfo = New ProcessStartInfo(My.Settings.filePath8)
                If args <> "" Then
                    startinfo.FileName = My.Settings.filePath8.Replace(" " & args, "")
                End If
                startinfo.Arguments = args.Replace(ControlChars.Quote, "")

                startinfo.UseShellExecute = True
                If My.Settings.windowState8 = 1 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Minimized
                ElseIf My.Settings.windowState8 = 2 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Maximized
                End If

                Process.Start(startinfo)
                System.Threading.Thread.Sleep(My.Settings.fileDelay8 * 1000)
            End If
        Catch ex As System.ComponentModel.Win32Exception
            blnError = True
        End Try
        Try
            If (My.Settings.disabled9 <> True) And (My.Settings.filePath9 <> Nothing) And ((isDoubleRun(0) = False And My.Settings.chbDoubleRun = True) Or My.Settings.chbDoubleRun = False) Then
                Dim r As New System.Text.RegularExpressions.Regex(".* (?<args>" & ControlChars.Quote & ".*\" & ControlChars.Quote & ")$", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                Dim m As System.Text.RegularExpressions.Match = r.Match(My.Settings.filePath9)
                Dim args As String = m.Groups("args").Value
                Dim startinfo = New ProcessStartInfo(My.Settings.filePath9)
                If args <> "" Then
                    startinfo.FileName = My.Settings.filePath9.Replace(" " & args, "")
                End If
                startinfo.Arguments = args.Replace(ControlChars.Quote, "")

                startinfo.UseShellExecute = True
                If My.Settings.windowState9 = 1 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Minimized
                ElseIf My.Settings.windowState9 = 2 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Maximized
                End If

                Process.Start(startinfo)
                System.Threading.Thread.Sleep(My.Settings.fileDelay9 * 1000)
            End If
        Catch ex As System.ComponentModel.Win32Exception
            blnError = True
        End Try
        Try
            If (My.Settings.disabled10 <> True) And (My.Settings.filePath10 <> Nothing) And ((isDoubleRun(0) = False And My.Settings.chbDoubleRun = True) Or My.Settings.chbDoubleRun = False) Then
                Dim r As New System.Text.RegularExpressions.Regex(".* (?<args>" & ControlChars.Quote & ".*\" & ControlChars.Quote & ")$", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                Dim m As System.Text.RegularExpressions.Match = r.Match(My.Settings.filePath10)
                Dim args As String = m.Groups("args").Value
                Dim startinfo = New ProcessStartInfo(My.Settings.filePath10)
                If args <> "" Then
                    startinfo.FileName = My.Settings.filePath10.Replace(" " & args, "")
                End If
                startinfo.Arguments = args.Replace(ControlChars.Quote, "")

                startinfo.UseShellExecute = True
                If My.Settings.windowState10 = 1 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Minimized
                ElseIf My.Settings.windowState10 = 2 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Maximized
                End If

                Process.Start(startinfo)
                System.Threading.Thread.Sleep(My.Settings.fileDelay10 * 1000)
            End If
        Catch ex As System.ComponentModel.Win32Exception
            blnError = True
        End Try
        Try
            If (My.Settings.disabled11 <> True) And (My.Settings.filePath11 <> Nothing) And ((isDoubleRun(0) = False And My.Settings.chbDoubleRun = True) Or My.Settings.chbDoubleRun = False) Then
                Dim r As New System.Text.RegularExpressions.Regex(".* (?<args>" & ControlChars.Quote & ".*\" & ControlChars.Quote & ")$", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                Dim m As System.Text.RegularExpressions.Match = r.Match(My.Settings.filePath11)
                Dim args As String = m.Groups("args").Value
                Dim startinfo = New ProcessStartInfo(My.Settings.filePath11)
                If args <> "" Then
                    startinfo.FileName = My.Settings.filePath11.Replace(" " & args, "")
                End If
                startinfo.Arguments = args.Replace(ControlChars.Quote, "")

                startinfo.UseShellExecute = True
                If My.Settings.windowState11 = 1 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Minimized
                ElseIf My.Settings.windowState11 = 2 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Maximized
                End If

                Process.Start(startinfo)
                System.Threading.Thread.Sleep(My.Settings.fileDelay11 * 1000)
            End If
        Catch ex As System.ComponentModel.Win32Exception
            blnError = True
        End Try
        Try
            If (My.Settings.disabled12 <> True) And (My.Settings.filePath12 <> Nothing) And ((isDoubleRun(0) = False And My.Settings.chbDoubleRun = True) Or My.Settings.chbDoubleRun = False) Then
                Dim r As New System.Text.RegularExpressions.Regex(".* (?<args>" & ControlChars.Quote & ".*\" & ControlChars.Quote & ")$", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                Dim m As System.Text.RegularExpressions.Match = r.Match(My.Settings.filePath12)
                Dim args As String = m.Groups("args").Value
                Dim startinfo = New ProcessStartInfo(My.Settings.filePath12)
                If args <> "" Then
                    startinfo.FileName = My.Settings.filePath12.Replace(" " & args, "")
                End If
                startinfo.Arguments = args.Replace(ControlChars.Quote, "")

                startinfo.UseShellExecute = True
                If My.Settings.windowState12 = 1 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Minimized
                ElseIf My.Settings.windowState12 = 2 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Maximized
                End If

                Process.Start(startinfo)
                System.Threading.Thread.Sleep(My.Settings.fileDelay12 * 1000)
            End If
        Catch ex As System.ComponentModel.Win32Exception
            blnError = True
        End Try
        Try
            If (My.Settings.disabled13 <> True) And (My.Settings.filePath13 <> Nothing) And ((isDoubleRun(0) = False And My.Settings.chbDoubleRun = True) Or My.Settings.chbDoubleRun = False) Then
                Dim r As New System.Text.RegularExpressions.Regex(".* (?<args>" & ControlChars.Quote & ".*\" & ControlChars.Quote & ")$", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                Dim m As System.Text.RegularExpressions.Match = r.Match(My.Settings.filePath13)
                Dim args As String = m.Groups("args").Value
                Dim startinfo = New ProcessStartInfo(My.Settings.filePath13)
                If args <> "" Then
                    startinfo.FileName = My.Settings.filePath13.Replace(" " & args, "")
                End If
                startinfo.Arguments = args.Replace(ControlChars.Quote, "")

                startinfo.UseShellExecute = True
                If My.Settings.windowState13 = 1 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Minimized
                ElseIf My.Settings.windowState13 = 2 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Maximized
                End If

                Process.Start(startinfo)
                System.Threading.Thread.Sleep(My.Settings.fileDelay13 * 1000)
            End If
        Catch ex As System.ComponentModel.Win32Exception
            blnError = True
        End Try
        Try
            If (My.Settings.disabled14 <> True) And (My.Settings.filePath14 <> Nothing) And ((isDoubleRun(0) = False And My.Settings.chbDoubleRun = True) Or My.Settings.chbDoubleRun = False) Then
                Dim r As New System.Text.RegularExpressions.Regex(".* (?<args>" & ControlChars.Quote & ".*\" & ControlChars.Quote & ")$", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                Dim m As System.Text.RegularExpressions.Match = r.Match(My.Settings.filePath14)
                Dim args As String = m.Groups("args").Value
                Dim startinfo = New ProcessStartInfo(My.Settings.filePath14)
                If args <> "" Then
                    startinfo.FileName = My.Settings.filePath14.Replace(" " & args, "")
                End If
                startinfo.Arguments = args.Replace(ControlChars.Quote, "")

                startinfo.UseShellExecute = True
                If My.Settings.windowState14 = 1 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Minimized
                ElseIf My.Settings.windowState14 = 2 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Maximized
                End If

                Process.Start(startinfo)
                System.Threading.Thread.Sleep(My.Settings.fileDelay14 * 1000)
            End If
        Catch ex As System.ComponentModel.Win32Exception
            blnError = True
        End Try
        Try
            If (My.Settings.disabled15 <> True) And (My.Settings.filePath15 <> Nothing) And ((isDoubleRun(0) = False And My.Settings.chbDoubleRun = True) Or My.Settings.chbDoubleRun = False) Then
                Dim r As New System.Text.RegularExpressions.Regex(".* (?<args>" & ControlChars.Quote & ".*\" & ControlChars.Quote & ")$", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                Dim m As System.Text.RegularExpressions.Match = r.Match(My.Settings.filePath15)
                Dim args As String = m.Groups("args").Value
                Dim startinfo = New ProcessStartInfo(My.Settings.filePath15)
                If args <> "" Then
                    startinfo.FileName = My.Settings.filePath15.Replace(" " & args, "")
                End If
                startinfo.Arguments = args.Replace(ControlChars.Quote, "")

                startinfo.UseShellExecute = True
                If My.Settings.windowState15 = 1 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Minimized
                ElseIf My.Settings.windowState15 = 2 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Maximized
                End If

                Process.Start(startinfo)
                System.Threading.Thread.Sleep(My.Settings.fileDelay15 * 1000)
            End If
        Catch ex As System.ComponentModel.Win32Exception
            blnError = True
        End Try
        Try
            If (My.Settings.disabled16 <> True) And (My.Settings.filePath16 <> Nothing) And ((isDoubleRun(0) = False And My.Settings.chbDoubleRun = True) Or My.Settings.chbDoubleRun = False) Then
                Dim r As New System.Text.RegularExpressions.Regex(".* (?<args>" & ControlChars.Quote & ".*\" & ControlChars.Quote & ")$", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                Dim m As System.Text.RegularExpressions.Match = r.Match(My.Settings.filePath16)
                Dim args As String = m.Groups("args").Value
                Dim startinfo = New ProcessStartInfo(My.Settings.filePath16)
                If args <> "" Then
                    startinfo.FileName = My.Settings.filePath16.Replace(" " & args, "")
                End If
                startinfo.Arguments = args.Replace(ControlChars.Quote, "")

                startinfo.UseShellExecute = True
                If My.Settings.windowState16 = 1 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Minimized
                ElseIf My.Settings.windowState16 = 2 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Maximized
                End If

                Process.Start(startinfo)
                System.Threading.Thread.Sleep(My.Settings.fileDelay16 * 1000)
            End If
        Catch ex As System.ComponentModel.Win32Exception
            blnError = True
        End Try
        Try
            If (My.Settings.disabled17 <> True) And (My.Settings.filePath17 <> Nothing) And ((isDoubleRun(0) = False And My.Settings.chbDoubleRun = True) Or My.Settings.chbDoubleRun = False) Then
                Dim r As New System.Text.RegularExpressions.Regex(".* (?<args>" & ControlChars.Quote & ".*\" & ControlChars.Quote & ")$", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                Dim m As System.Text.RegularExpressions.Match = r.Match(My.Settings.filePath17)
                Dim args As String = m.Groups("args").Value
                Dim startinfo = New ProcessStartInfo(My.Settings.filePath17)
                If args <> "" Then
                    startinfo.FileName = My.Settings.filePath17.Replace(" " & args, "")
                End If
                startinfo.Arguments = args.Replace(ControlChars.Quote, "")

                startinfo.UseShellExecute = True
                If My.Settings.windowState17 = 1 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Minimized
                ElseIf My.Settings.windowState17 = 2 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Maximized
                End If

                Process.Start(startinfo)
                System.Threading.Thread.Sleep(My.Settings.fileDelay17 * 1000)
            End If
        Catch ex As System.ComponentModel.Win32Exception
            blnError = True
        End Try
        Try
            If (My.Settings.disabled18 <> True) And (My.Settings.filePath18 <> Nothing) And ((isDoubleRun(0) = False And My.Settings.chbDoubleRun = True) Or My.Settings.chbDoubleRun = False) Then
                Dim r As New System.Text.RegularExpressions.Regex(".* (?<args>" & ControlChars.Quote & ".*\" & ControlChars.Quote & ")$", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                Dim m As System.Text.RegularExpressions.Match = r.Match(My.Settings.filePath18)
                Dim args As String = m.Groups("args").Value
                Dim startinfo = New ProcessStartInfo(My.Settings.filePath18)
                If args <> "" Then
                    startinfo.FileName = My.Settings.filePath18.Replace(" " & args, "")
                End If
                startinfo.Arguments = args.Replace(ControlChars.Quote, "")

                startinfo.UseShellExecute = True
                If My.Settings.windowState18 = 1 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Minimized
                ElseIf My.Settings.windowState18 = 2 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Maximized
                End If

                Process.Start(startinfo)
                System.Threading.Thread.Sleep(My.Settings.fileDelay18 * 1000)
            End If
        Catch ex As System.ComponentModel.Win32Exception
            blnError = True
        End Try
        Try
            If (My.Settings.disabled19 <> True) And (My.Settings.filePath19 <> Nothing) And ((isDoubleRun(0) = False And My.Settings.chbDoubleRun = True) Or My.Settings.chbDoubleRun = False) Then
                Dim r As New System.Text.RegularExpressions.Regex(".* (?<args>" & ControlChars.Quote & ".*\" & ControlChars.Quote & ")$", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                Dim m As System.Text.RegularExpressions.Match = r.Match(My.Settings.filePath19)
                Dim args As String = m.Groups("args").Value
                Dim startinfo = New ProcessStartInfo(My.Settings.filePath19)
                If args <> "" Then
                    startinfo.FileName = My.Settings.filePath19.Replace(" " & args, "")
                End If
                startinfo.Arguments = args.Replace(ControlChars.Quote, "")

                startinfo.UseShellExecute = True
                If My.Settings.windowState19 = 1 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Minimized
                ElseIf My.Settings.windowState19 = 2 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Maximized
                End If

                Process.Start(startinfo)
                System.Threading.Thread.Sleep(My.Settings.fileDelay19 * 1000)
            End If
        Catch ex As System.ComponentModel.Win32Exception
            blnError = True
        End Try
        Try
            If (My.Settings.disabled20 <> True) And (My.Settings.filePath20 <> Nothing) And ((isDoubleRun(0) = False And My.Settings.chbDoubleRun = True) Or My.Settings.chbDoubleRun = False) Then
                Dim r As New System.Text.RegularExpressions.Regex(".* (?<args>" & ControlChars.Quote & ".*\" & ControlChars.Quote & ")$", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                Dim m As System.Text.RegularExpressions.Match = r.Match(My.Settings.filePath20)
                Dim args As String = m.Groups("args").Value
                Dim startinfo = New ProcessStartInfo(My.Settings.filePath20)
                If args <> "" Then
                    startinfo.FileName = My.Settings.filePath20.Replace(" " & args, "")
                End If
                startinfo.Arguments = args.Replace(ControlChars.Quote, "")

                startinfo.UseShellExecute = True
                If My.Settings.windowState20 = 1 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Minimized
                ElseIf My.Settings.windowState20 = 2 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Maximized
                End If

                Process.Start(startinfo)
                System.Threading.Thread.Sleep(My.Settings.fileDelay20 * 1000)
            End If
        Catch ex As System.ComponentModel.Win32Exception
            blnError = True
        End Try
        Try
            If (My.Settings.disabled21 <> True) And (My.Settings.filePath21 <> Nothing) And ((isDoubleRun(0) = False And My.Settings.chbDoubleRun = True) Or My.Settings.chbDoubleRun = False) Then
                Dim r As New System.Text.RegularExpressions.Regex(".* (?<args>" & ControlChars.Quote & ".*\" & ControlChars.Quote & ")$", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                Dim m As System.Text.RegularExpressions.Match = r.Match(My.Settings.filePath21)
                Dim args As String = m.Groups("args").Value
                Dim startinfo = New ProcessStartInfo(My.Settings.filePath21)
                If args <> "" Then
                    startinfo.FileName = My.Settings.filePath21.Replace(" " & args, "")
                End If
                startinfo.Arguments = args.Replace(ControlChars.Quote, "")

                startinfo.UseShellExecute = True
                If My.Settings.windowState21 = 1 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Minimized
                ElseIf My.Settings.windowState21 = 2 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Maximized
                End If

                Process.Start(startinfo)
                System.Threading.Thread.Sleep(My.Settings.fileDelay21 * 1000)
            End If
        Catch ex As System.ComponentModel.Win32Exception
            blnError = True
        End Try
        Try
            If (My.Settings.disabled22 <> True) And (My.Settings.filePath22 <> Nothing) And ((isDoubleRun(0) = False And My.Settings.chbDoubleRun = True) Or My.Settings.chbDoubleRun = False) Then
                Dim r As New System.Text.RegularExpressions.Regex(".* (?<args>" & ControlChars.Quote & ".*\" & ControlChars.Quote & ")$", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                Dim m As System.Text.RegularExpressions.Match = r.Match(My.Settings.filePath22)
                Dim args As String = m.Groups("args").Value
                Dim startinfo = New ProcessStartInfo(My.Settings.filePath22)
                If args <> "" Then
                    startinfo.FileName = My.Settings.filePath22.Replace(" " & args, "")
                End If
                startinfo.Arguments = args.Replace(ControlChars.Quote, "")

                startinfo.UseShellExecute = True
                If My.Settings.windowState22 = 1 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Minimized
                ElseIf My.Settings.windowState22 = 2 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Maximized
                End If

                Process.Start(startinfo)
                System.Threading.Thread.Sleep(My.Settings.fileDelay22 * 1000)
            End If
        Catch ex As System.ComponentModel.Win32Exception
            blnError = True
        End Try
        Try
            If (My.Settings.disabled23 <> True) And (My.Settings.filePath23 <> Nothing) And ((isDoubleRun(0) = False And My.Settings.chbDoubleRun = True) Or My.Settings.chbDoubleRun = False) Then
                Dim r As New System.Text.RegularExpressions.Regex(".* (?<args>" & ControlChars.Quote & ".*\" & ControlChars.Quote & ")$", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                Dim m As System.Text.RegularExpressions.Match = r.Match(My.Settings.filePath23)
                Dim args As String = m.Groups("args").Value
                Dim startinfo = New ProcessStartInfo(My.Settings.filePath23)
                If args <> "" Then
                    startinfo.FileName = My.Settings.filePath23.Replace(" " & args, "")
                End If
                startinfo.Arguments = args.Replace(ControlChars.Quote, "")

                startinfo.UseShellExecute = True
                If My.Settings.windowState23 = 1 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Minimized
                ElseIf My.Settings.windowState23 = 2 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Maximized
                End If

                Process.Start(startinfo)
                System.Threading.Thread.Sleep(My.Settings.fileDelay23 * 1000)
            End If
        Catch ex As System.ComponentModel.Win32Exception
            blnError = True
        End Try
        Try
            If (My.Settings.disabled24 <> True) And (My.Settings.filePath24 <> Nothing) And ((isDoubleRun(0) = False And My.Settings.chbDoubleRun = True) Or My.Settings.chbDoubleRun = False) Then
                Dim r As New System.Text.RegularExpressions.Regex(".* (?<args>" & ControlChars.Quote & ".*\" & ControlChars.Quote & ")$", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                Dim m As System.Text.RegularExpressions.Match = r.Match(My.Settings.filePath24)
                Dim args As String = m.Groups("args").Value
                Dim startinfo = New ProcessStartInfo(My.Settings.filePath24)
                If args <> "" Then
                    startinfo.FileName = My.Settings.filePath24.Replace(" " & args, "")
                End If
                startinfo.Arguments = args.Replace(ControlChars.Quote, "")

                startinfo.UseShellExecute = True
                If My.Settings.windowState24 = 1 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Minimized
                ElseIf My.Settings.windowState24 = 2 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Maximized
                End If

                Process.Start(startinfo)
                System.Threading.Thread.Sleep(My.Settings.fileDelay24 * 1000)
            End If
        Catch ex As System.ComponentModel.Win32Exception
            blnError = True
        End Try
        Try
            If (My.Settings.disabled25 <> True) And (My.Settings.filePath25 <> Nothing) And ((isDoubleRun(0) = False And My.Settings.chbDoubleRun = True) Or My.Settings.chbDoubleRun = False) Then
                Dim r As New System.Text.RegularExpressions.Regex(".* (?<args>" & ControlChars.Quote & ".*\" & ControlChars.Quote & ")$", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                Dim m As System.Text.RegularExpressions.Match = r.Match(My.Settings.filePath25)
                Dim args As String = m.Groups("args").Value
                Dim startinfo = New ProcessStartInfo(My.Settings.filePath25)
                If args <> "" Then
                    startinfo.FileName = My.Settings.filePath25.Replace(" " & args, "")
                End If
                startinfo.Arguments = args.Replace(ControlChars.Quote, "")

                startinfo.UseShellExecute = True
                If My.Settings.windowState25 = 1 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Minimized
                ElseIf My.Settings.windowState25 = 2 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Maximized
                End If

                Process.Start(startinfo)
                System.Threading.Thread.Sleep(My.Settings.fileDelay25 * 1000)
            End If
        Catch ex As System.ComponentModel.Win32Exception
            blnError = True
        End Try
        Try
            If (My.Settings.disabled26 <> True) And (My.Settings.filePath26 <> Nothing) And ((isDoubleRun(0) = False And My.Settings.chbDoubleRun = True) Or My.Settings.chbDoubleRun = False) Then
                Dim r As New System.Text.RegularExpressions.Regex(".* (?<args>" & ControlChars.Quote & ".*\" & ControlChars.Quote & ")$", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                Dim m As System.Text.RegularExpressions.Match = r.Match(My.Settings.filePath26)
                Dim args As String = m.Groups("args").Value
                Dim startinfo = New ProcessStartInfo(My.Settings.filePath26)
                If args <> "" Then
                    startinfo.FileName = My.Settings.filePath26.Replace(" " & args, "")
                End If
                startinfo.Arguments = args.Replace(ControlChars.Quote, "")

                startinfo.UseShellExecute = True
                If My.Settings.windowState26 = 1 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Minimized
                ElseIf My.Settings.windowState26 = 2 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Maximized
                End If

                Process.Start(startinfo)
                System.Threading.Thread.Sleep(My.Settings.fileDelay26 * 1000)
            End If
        Catch ex As System.ComponentModel.Win32Exception
            blnError = True
        End Try
        Try
            If (My.Settings.disabled27 <> True) And (My.Settings.filePath27 <> Nothing) And ((isDoubleRun(0) = False And My.Settings.chbDoubleRun = True) Or My.Settings.chbDoubleRun = False) Then
                Dim r As New System.Text.RegularExpressions.Regex(".* (?<args>" & ControlChars.Quote & ".*\" & ControlChars.Quote & ")$", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                Dim m As System.Text.RegularExpressions.Match = r.Match(My.Settings.filePath27)
                Dim args As String = m.Groups("args").Value
                Dim startinfo = New ProcessStartInfo(My.Settings.filePath27)
                If args <> "" Then
                    startinfo.FileName = My.Settings.filePath27.Replace(" " & args, "")
                End If
                startinfo.Arguments = args.Replace(ControlChars.Quote, "")

                startinfo.UseShellExecute = True
                If My.Settings.windowState27 = 1 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Minimized
                ElseIf My.Settings.windowState27 = 2 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Maximized
                End If

                Process.Start(startinfo)
                System.Threading.Thread.Sleep(My.Settings.fileDelay27 * 1000)
            End If
        Catch ex As System.ComponentModel.Win32Exception
            blnError = True
        End Try
        Try
            If (My.Settings.disabled28 <> True) And (My.Settings.filePath28 <> Nothing) And ((isDoubleRun(0) = False And My.Settings.chbDoubleRun = True) Or My.Settings.chbDoubleRun = False) Then
                Dim r As New System.Text.RegularExpressions.Regex(".* (?<args>" & ControlChars.Quote & ".*\" & ControlChars.Quote & ")$", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                Dim m As System.Text.RegularExpressions.Match = r.Match(My.Settings.filePath28)
                Dim args As String = m.Groups("args").Value
                Dim startinfo = New ProcessStartInfo(My.Settings.filePath28)
                If args <> "" Then
                    startinfo.FileName = My.Settings.filePath28.Replace(" " & args, "")
                End If
                startinfo.Arguments = args.Replace(ControlChars.Quote, "")

                startinfo.UseShellExecute = True
                If My.Settings.windowState28 = 1 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Minimized
                ElseIf My.Settings.windowState28 = 2 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Maximized
                End If

                Process.Start(startinfo)
                System.Threading.Thread.Sleep(My.Settings.fileDelay28 * 1000)
            End If
        Catch ex As System.ComponentModel.Win32Exception
            blnError = True
        End Try
        Try
            If (My.Settings.disabled29 <> True) And (My.Settings.filePath29 <> Nothing) And ((isDoubleRun(0) = False And My.Settings.chbDoubleRun = True) Or My.Settings.chbDoubleRun = False) Then
                Dim r As New System.Text.RegularExpressions.Regex(".* (?<args>" & ControlChars.Quote & ".*\" & ControlChars.Quote & ")$", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                Dim m As System.Text.RegularExpressions.Match = r.Match(My.Settings.filePath29)
                Dim args As String = m.Groups("args").Value
                Dim startinfo = New ProcessStartInfo(My.Settings.filePath29)
                If args <> "" Then
                    startinfo.FileName = My.Settings.filePath29.Replace(" " & args, "")
                End If
                startinfo.Arguments = args.Replace(ControlChars.Quote, "")

                startinfo.UseShellExecute = True
                If My.Settings.windowState29 = 1 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Minimized
                ElseIf My.Settings.windowState29 = 2 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Maximized
                End If

                Process.Start(startinfo)
                System.Threading.Thread.Sleep(My.Settings.fileDelay29 * 1000)
            End If
        Catch ex As System.ComponentModel.Win32Exception
            blnError = True
        End Try
        Try
            If (My.Settings.disabled30 <> True) And (My.Settings.filePath30 <> Nothing) And ((isDoubleRun(0) = False And My.Settings.chbDoubleRun = True) Or My.Settings.chbDoubleRun = False) Then
                Dim r As New System.Text.RegularExpressions.Regex(".* (?<args>" & ControlChars.Quote & ".*\" & ControlChars.Quote & ")$", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                Dim m As System.Text.RegularExpressions.Match = r.Match(My.Settings.filePath30)
                Dim args As String = m.Groups("args").Value
                Dim startinfo = New ProcessStartInfo(My.Settings.filePath30)
                If args <> "" Then
                    startinfo.FileName = My.Settings.filePath30.Replace(" " & args, "")
                End If
                startinfo.Arguments = args.Replace(ControlChars.Quote, "")

                startinfo.UseShellExecute = True
                If My.Settings.windowState30 = 1 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Minimized
                ElseIf My.Settings.windowState30 = 2 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Maximized
                End If

                Process.Start(startinfo)
                System.Threading.Thread.Sleep(My.Settings.fileDelay30 * 1000)
            End If
        Catch ex As System.ComponentModel.Win32Exception
            blnError = True
        End Try
        Try
            If (My.Settings.disabled31 <> True) And (My.Settings.filePath31 <> Nothing) And ((isDoubleRun(0) = False And My.Settings.chbDoubleRun = True) Or My.Settings.chbDoubleRun = False) Then
                Dim r As New System.Text.RegularExpressions.Regex(".* (?<args>" & ControlChars.Quote & ".*\" & ControlChars.Quote & ")$", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                Dim m As System.Text.RegularExpressions.Match = r.Match(My.Settings.filePath31)
                Dim args As String = m.Groups("args").Value
                Dim startinfo = New ProcessStartInfo(My.Settings.filePath31)
                If args <> "" Then
                    startinfo.FileName = My.Settings.filePath31.Replace(" " & args, "")
                End If
                startinfo.Arguments = args.Replace(ControlChars.Quote, "")

                startinfo.UseShellExecute = True
                If My.Settings.windowState31 = 1 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Minimized
                ElseIf My.Settings.windowState31 = 2 Then
                    startinfo.WindowStyle = ProcessWindowStyle.Maximized
                End If

                Process.Start(startinfo)
                System.Threading.Thread.Sleep(My.Settings.fileDelay31 * 1000)
            End If
        Catch ex As System.ComponentModel.Win32Exception
            blnError = True
        End Try

        If blnError = True Then
            MessageBox.Show("ファイルパスが正しく入力されていないため起動できないものがありました。" & vbCrLf & "ご確認ください。", "なにかがうまくいきませんでした", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

    End Sub

    Public Sub closeAllApp()
        Dim ps As Process() = Process.GetProcesses()
        Dim blnError As Boolean = False
        Dim errorProcess = New List(Of String)
        For Each p As System.Diagnostics.Process In ps
            Try
                p.CloseMainWindow()
                p.WaitForExit(10000)
            Catch ex As Exception
                blnError = True
            End Try
        Next
        If blnError = True Then
            MessageBox.Show("なにかしらエラーが発生した気もしますが閉じれるものは頑張って閉じました。" & vbCrLf & vbCrLf & "情報のご提供をいただける場合は下記のエラーコードと共に みるみ までご連絡ください。" & vbCrLf & vbCrLf & "ErrorCode : 0001", "ちょっとエラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Public Function searchInstallAppIcon(ByVal strAppPathTemp) As Bitmap
        Dim r As New System.Text.RegularExpressions.Regex(".* (?<args>" & ControlChars.Quote & ".*\" & ControlChars.Quote & ")$", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Dim m As System.Text.RegularExpressions.Match = r.Match(strAppPathTemp)
        Dim args As String = m.Groups("args").Value
        Dim strAppPath As String = strAppPathTemp
        If args <> "" Then
            strAppPath = strAppPath.Replace(" " & args, "")
        End If
        Dim bmpAppIcon As Bitmap = Nothing
        Dim shinfo As New SHFILEINFO()

        Dim hSuccess As IntPtr = SHGetFileInfo(strAppPath, 0, shinfo, Marshal.SizeOf(shinfo), SHGFI_ICON Or SHGFI_LARGEICON)
        If hSuccess.Equals(IntPtr.Zero) = False Then
            Dim appIcon As Icon = Icon.FromHandle(shinfo.hIcon)
            bmpAppIcon = appIcon.ToBitmap()
        End If
        Return bmpAppIcon
    End Function

    Public Sub testRegKey()
        ' 操作するレジストリ・キーの名前
        Dim rKeyName As String = "SOFTWARE\Microsoft\.NETFramework"
        ' 取得処理を行う対象となるレジストリの値の名前
        Dim rGetValueName As String = "InstallRoot"

        ' レジストリの取得
        Try
            ' レジストリ・キーのパスを指定してレジストリを開く
            Dim rKey As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(rKeyName)

            ' レジストリの値を取得
            Dim location As String = CStr(rKey.GetValue(rGetValueName))

            rKey.Close()

            MessageBox.Show(location, "確認リスト", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)

        Catch ex As NullReferenceException
            Console.WriteLine("レジストリ［" + rKeyName + "］の［" + rGetValueName + "］がありません！")
        End Try
    End Sub

    'Public Function isDefaultAllSettings()
    '    If () And
    '            () And
    '            () And
    '            () Then
    '        Return True
    '    End If
    '    Return False
    'End Function
End Module