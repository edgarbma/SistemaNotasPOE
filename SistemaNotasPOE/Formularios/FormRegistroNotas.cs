using System;
using System.Drawing;
using System.Windows.Forms;
using SistemaNotasPOE.Clases;
using SistemaNotasPOE.Repositorios;

namespace SistemaNotasPOE.Formularios
{
    public class FormRegistroNotas : Form
    {
        private Label lblNombre = null!, lblApellido = null!, lblCodigo = null!;
        private Label lblLaboratorio = null!, lblParcial = null!, lblAsistencia = null!;
        private TextBox txtNombre = null!, txtApellido = null!, txtCodigo = null!;
        private TextBox txtLaboratorio = null!, txtParcial = null!, txtAsistencia = null!;
        private Button btnAgregar = null!;
        private DataGridView dgvRegistro = null!;

        private readonly EstudianteRepo _repo;

        public FormRegistroNotas()
        {
            _repo = new EstudianteRepo();
            InicializarComponentes();
        }

        private void InicializarComponentes()
        {
            this.Text = "Crear Estudiante";
            this.Size = new Size(720, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            lblNombre = new Label { Text = "Nombre:", Location = new Point(20, 20), AutoSize = true };
            txtNombre = new TextBox { Name = "txtNombre", Location = new Point(110, 18), Width = 200 };

            lblApellido = new Label { Text = "Apellido:", Location = new Point(340, 20), AutoSize = true };
            txtApellido = new TextBox { Name = "txtApellido", Location = new Point(430, 18), Width = 200 };

            lblCodigo = new Label { Text = "Código:", Location = new Point(20, 60), AutoSize = true };
            txtCodigo = new TextBox { Name = "txtCodigo", Location = new Point(110, 58), Width = 200 };

            lblLaboratorio = new Label { Text = "Laboratorio:", Location = new Point(340, 60), AutoSize = true };
            txtLaboratorio = new TextBox { Name = "txtLaboratorio", Location = new Point(430, 58), Width = 200 };

            lblParcial = new Label { Text = "Parcial:", Location = new Point(20, 100), AutoSize = true };
            txtParcial = new TextBox { Name = "txtParcial", Location = new Point(110, 98), Width = 200 };

            lblAsistencia = new Label { Text = "Asistencia:", Location = new Point(340, 100), AutoSize = true };
            txtAsistencia = new TextBox { Name = "txtAsistencia", Location = new Point(430, 98), Width = 200 };

            btnAgregar = new Button { Name = "btnAgregar", Text = "Agregar", Location = new Point(430, 138), Width = 200, Height = 32 };

            dgvRegistro = new DataGridView
            {
                Name = "dgvRegistro",
                Location = new Point(20, 185),
                Size = new Size(660, 250),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            txtLaboratorio.KeyPress += ValidarSoloNumerosDecimales;
            txtParcial.KeyPress += ValidarSoloNumerosDecimales;
            txtAsistencia.KeyPress += ValidarSoloNumerosDecimales;

            btnAgregar.Click += BtnAgregar_Click;
            dgvRegistro.CellDoubleClick += DgvRegistro_CellDoubleClick;
            this.Load += FormRegistroNotas_Load;

            this.Controls.Add(lblNombre);
            this.Controls.Add(txtNombre);
            this.Controls.Add(lblApellido);
            this.Controls.Add(txtApellido);
            this.Controls.Add(lblCodigo);
            this.Controls.Add(txtCodigo);
            this.Controls.Add(lblLaboratorio);
            this.Controls.Add(txtLaboratorio);
            this.Controls.Add(lblParcial);
            this.Controls.Add(txtParcial);
            this.Controls.Add(lblAsistencia);
            this.Controls.Add(txtAsistencia);
            this.Controls.Add(btnAgregar);
            this.Controls.Add(dgvRegistro);
        }

        private void FormRegistroNotas_Load(object? sender, EventArgs e)
        {
            LimpiarControles();
            CargarGrid();
        }

        private void CargarGrid()
        {
            dgvRegistro.DataSource = null;
            dgvRegistro.DataSource = _repo.ObtenerTodos();
        }

        private void LimpiarControles()
        {
            txtNombre.Clear();
            txtApellido.Clear();
            txtCodigo.Clear();
            txtLaboratorio.Clear();
            txtParcial.Clear();
            txtAsistencia.Clear();
        }

        private void ValidarSoloNumerosDecimales(object? sender, KeyPressEventArgs e)
        {
            TextBox textBox = (TextBox)sender!;
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '.') && (textBox.Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void BtnAgregar_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellido.Text) ||
                string.IsNullOrWhiteSpace(txtCodigo.Text) ||
                string.IsNullOrWhiteSpace(txtLaboratorio.Text) ||
                string.IsNullOrWhiteSpace(txtParcial.Text) ||
                string.IsNullOrWhiteSpace(txtAsistencia.Text))
            {
                MessageBox.Show("Todos los campos son obligatorios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtLaboratorio.Text, out decimal lab) || lab < 0m || lab > 10.0m ||
                !decimal.TryParse(txtParcial.Text, out decimal par) || par < 0m || par > 10.0m ||
                !decimal.TryParse(txtAsistencia.Text, out decimal asi) || asi < 0m || asi > 10.0m)
            {
                MessageBox.Show("Las notas deben estar en un rango de 0.0 a 10.0.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal notaFinal = (lab * 0.5m) + (par * 0.4m) + (asi * 0.1m);

            Estudiante estudiante = new Estudiante
            {
                Nombre = txtNombre.Text.Trim().ToUpper(),
                Apellido = txtApellido.Text.Trim().ToUpper(),
                Codigo = txtCodigo.Text.Trim().ToUpper(),
                Laboratorio = lab,
                Parcial = par,
                Asistencia = asi,
                NotaFinal = Math.Round(notaFinal, 2)
            };

            _repo.Insertar(estudiante);
            LimpiarControles();
            CargarGrid();
        }

        private void DgvRegistro_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                Estudiante estudianteSeleccionado = (Estudiante)dgvRegistro.Rows[e.RowIndex].DataBoundItem;
                using (FormEditarEstudiante frmEditar = new FormEditarEstudiante(estudianteSeleccionado))
                {
                    if (frmEditar.ShowDialog() == DialogResult.OK)
                    {
                        CargarGrid();
                    }
                }
            }
        }
    }
}