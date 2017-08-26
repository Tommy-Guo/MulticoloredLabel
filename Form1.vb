Public Class Form1 

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ListBox1.Items.Add(String.Format("StartIndex: {0} |  Length: {1} |  Color: {2}", TextBox1.Text, TextBox2.Text, Panel1.BackColor.R & ", " & Panel1.BackColor.G & ", " & Panel1.BackColor.B))
        Button3.PerformClick()
    End Sub

    Private Sub Panel1_Click(sender As Object, e As EventArgs) Handles Panel1.Click
        Dim nColorDialog As New ColorDialog
        If nColorDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            Panel1.BackColor = nColorDialog.Color
        End If
    End Sub 

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        MultiLabel1._charSelection.Clear()

        For Each charInfo As String In ListBox1.Items
            Dim charInfoString() As String = charInfo.Replace("StartIndex:", "").Replace("Length:", "").Replace("Color:", "").Replace(" ", "").Split("|"c)
            Dim nSelection As New CharSelection
            Dim colorSplit() As String = charInfoString(2).Split(","c)
            nSelection.Color = Color.FromArgb(colorSplit(0), colorSplit(1), colorSplit(2))
            nSelection.StartIndex = Convert.ToInt32(charInfoString(0))
            nSelection.Length = Convert.ToInt32(charInfoString(1))
            MultiLabel1._charSelection.Add(nSelection)
        Next

        MultiLabel1.Invalidate()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            ListBox1.Items.Remove(ListBox1.SelectedItem)
        Catch
        End Try
    End Sub
End Class

Public Class MultiLabel
    Inherits Control

    Public _charSelection As New List(Of CharSelection) 

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim g As Graphics = e.Graphics
        g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
        g.Clear(BackColor)

        Dim txtChars As Char() = Text.ToArray

        If _charSelection.Count = 0 Then

            g.DrawString(Text, Font, Brushes.Black, New Rectangle(3, 3, Width - 3, Height - 3))

        Else

            Dim x, y As Integer
            For i As Integer = 0 To txtChars.Count - 1
                For c As Integer = 0 To _charSelection.Count - 1
                    If i + 1 > _charSelection(c).StartIndex And i < _charSelection(c).StartIndex + _charSelection(c).Length Then
                        g.DrawString(txtChars(i), Font, New SolidBrush(_charSelection(c).Color), x, y)
                        Exit For
                    Else
                        g.DrawString(txtChars(i), Font, Brushes.Black, x, y)
                    End If
                Next
                x += g.MeasureString(txtChars(i), Font).Width
                If y > Width - 20 Then
                    x = 0
                    y += g.MeasureString(txtChars(i), Font).Height + 2
                End If
            Next

        End If

    End Sub 
End Class

Public Class CharSelection

    Public Color As Color = Color.Black
    Public StartIndex As Integer = 1
    Public Length As Integer = 1
End Class