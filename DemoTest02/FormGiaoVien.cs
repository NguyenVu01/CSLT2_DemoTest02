using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DemoTest02
{
    public partial class FormGiaoVien : Form
    {
        DataTable tblGV = new DataTable();
        public FormGiaoVien()
        {
            DAO.Connect();
            InitializeComponent();
        }
        private void Load_dataGridView()
        {
            string sql;
            sql = "SELECT * FROM tblGiaoVien";
            tblGV = DAO.LoadDataToTable(sql);
            dataGridView.DataSource = tblGV;
            dataGridView.Columns[0].HeaderText = "Mã giáo viên";
            dataGridView.Columns[1].HeaderText = "Họ và tên";
            dataGridView.Columns[2].HeaderText = "Quê quán";
            dataGridView.AllowUserToAddRows = false;
            dataGridView.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void FormGiaoVien_Load(object sender, EventArgs e)
        {
            txtMaGV.Enabled = false;
            btnLuu.Enabled = false;
            btnHuy.Enabled = false;
            Load_dataGridView();
        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaGV.Text = dataGridView.CurrentRow.Cells[0].Value.ToString();
            txtHoTen.Text = dataGridView.CurrentRow.Cells[1].Value.ToString();
            txtQueQuan.Text = dataGridView.CurrentRow.Cells[2].Value.ToString();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            ResetValues();
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnThem.Enabled = false;
            btnHuy.Enabled = true;
            btnLuu.Enabled = true;
            txtMaGV.Enabled = true;
            txtMaGV.Focus();
        }
        private void ResetValues()
        {
            txtMaGV.Text = "";
            txtHoTen.Text = "";
            txtQueQuan.Text = "";
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (tblGV.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu!", "Thông báo", MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }
            if (txtMaGV.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào", "Thông báo",MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtHoTen.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên giáo viên", "Thông báo",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return;
            }
            if (txtQueQuan.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập quê quán", "Thông báo", MessageBoxButtons.OK,MessageBoxIcon.Warning);
                txtQueQuan.Focus();
                return;
            }
            try
            {
                string sql = "UPDATE tblGiaoVien SET  HoTen=N'" + txtHoTen.Text.Trim().ToString() +
                             "',QueQuan= N'" + txtQueQuan.Text.Trim().ToString() +
                             "' WHERE MaGV= '" + txtMaGV.Text + "'";
                DAO.RunSql(sql);
                Load_dataGridView();
                ResetValues();
                btnHuy.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã có lỗi gì đó! " + ex.Message, "Báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string sql;
            if (tblGV.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtMaGV.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Bạn có muốn xóa không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                sql = "DELETE tblGiaoVien WHERE MaGV = '" + txtMaGV.Text + "'";
                DAO.RunSqlDel(sql);
                Load_dataGridView();
                ResetValues();
            }

        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql;
            if (txtMaGV.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập mã giáo viên", "Thông báo",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaGV.Focus();
                return;
            }
            if (txtHoTen.Text.Trim().Length == 0)
            {
                MessageBox.Show("Tên giáo viên sẽ được tự động đặt theo tên bạn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Text = "Nguyễn Văn A";
            }
            sql = "SELECT MaGV FROM tblGiaoVien WHERE MaGV ='" + txtMaGV.Text.Trim() + "'";
            if (DAO.CheckKey(sql))
            {
                MessageBox.Show("Mã giáo viên này đã có, bạn phải nhập mã khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaGV.Focus();
                txtMaGV.Text = "";
                return;
            }
            sql = "INSERT INTO tblGiaoVien(MaGV, HoTen, QueQuan) VALUES('" + txtMaGV.Text + "',N'" + txtHoTen.Text + "',N'" + txtQueQuan.Text + "')";
            try
            {
                DAO.RunSql(sql);
                Load_dataGridView();
                ResetValues();
                btnXoa.Enabled = true;
                btnThem.Enabled = true;
                btnSua.Enabled = true;
                btnHuy.Enabled = false;
                btnLuu.Enabled = false;
                txtMaGV.Enabled = false;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Đã có lỗi gì đó! " + ex.Message, "Báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            ResetValues();
            btnHuy.Enabled = false;
            btnThem.Enabled = true;
            btnXoa.Enabled = true;
            btnSua.Enabled = true;
            btnLuu.Enabled = false;
            txtMaGV.Enabled = false;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
            DAO.Close();
        }
    }
}
