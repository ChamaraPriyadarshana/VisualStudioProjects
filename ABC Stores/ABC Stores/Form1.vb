Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim frmProducts As New Products

        Guna2ShadowPanel1.Controls.Add(frmProducts)

        frmProducts.BringToFront()
        frmProducts.Show()
    End Sub
End Class
