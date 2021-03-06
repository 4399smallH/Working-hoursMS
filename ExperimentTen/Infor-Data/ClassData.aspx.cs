﻿using System;
using System.Web.UI.WebControls;
using System.Data;

namespace WHMS.Infor_Data
{
    public partial class ClassData : System.Web.UI.Page
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SessionManager.CheckLogin("../login.aspx");
                BindGrid(GridView1);
                GridView1.Caption = Session["Class"].ToString() + "班\t" + Session["SySe"].ToString() + "学期工时表";
            }        
        }
 

        public void Bind(DataTable data)
        {
            // DataTable data = new DataTable();
            DataRow dr;

            string sql1 = "select * from [Working_hoursInfor] where SySe like '%" + Session["SySe"].ToString() + "%'";
            string sql2 = "select StuID,StuName,Class from Student where Class like '%" + Session["Class"].ToString() + "%' order by StuID";
            string sql3 = "select distinct Program,Date from [Working_hoursInfor] where SySe like '%" + Session["SySe"].ToString() + "%'";
            DataTable dt = Common.datatable(sql1);
            DataTable student = Common.datatable(sql2);
            DataTable program = Common.datatable(sql3);

            data.Columns.Add("学号", typeof(string));
            data.Columns.Add("姓名", typeof(string));
            data.Columns.Add("班级", typeof(string));

            for (int i = 0; i <= program.Rows.Count; i++)
            {

                if (i < program.Rows.Count)
                {
                   
                    data.Columns.Add(program.Rows[i][0].ToString(), typeof(string));
                }

                else
                {
                    data.Columns.Add("合计", typeof(int));
                }
            }
            for (int i = 0; i < student.Rows.Count; i++)
            {
                dr = data.NewRow();
                dr[0] = student.Rows[i][0].ToString();
                dr[1] = student.Rows[i][1].ToString();
                dr[2] = student.Rows[i][2].ToString();

                int total = 0;
                for (int j = 0; j < program.Rows.Count; j++)
                {
                    for (int t = 0; t < dt.Rows.Count; t++)
                    {
                        string t1 = dt.Rows[t][0].ToString();
                        string t2 = student.Rows[i][0].ToString();
                        string t3 = dt.Rows[t][2].ToString();
                        string t4 = program.Rows[j][0].ToString();
                        if (dt.Rows[t][0].ToString() == student.Rows[i][0].ToString() && dt.Rows[t][2].ToString() == program.Rows[j][0].ToString())
                        {
                            // dr[program.Rows[j][0].ToString()]= dt.Rows[t][3].ToString();
                            dr[3 + j] = dt.Rows[t][3].ToString();
                            total += Convert.ToInt32(dt.Rows[t][3].ToString());
                        }
                    }
                }
                dr["合计"] = total;
                data.Rows.Add(dr);
            }

            //   GridView1.DataSource = data;
            //  GridView1.DataBind();
        }


        private void BindGrid(GridView GridView1)
        {
            #region  添加动态列   
            /*    GridView1.Columns.Clear();
                 GridView1.Width = new Unit(0);

                 string sql1 = "select * from [Working_hoursInfor] where SySe like '%2016-2017-1%'";
                 string sql2 = "select StuID,StuName,Class from Student where Class='信管1501' order by Class,StuID";
                 string sql3 = "select distinct Program,Date from [Working_hoursInfor] where SySe like '%2016-2017-1%'";
                 DataTable dt = Common.datatable(sql1);                                                                                                                                          
                 DataTable student = Common.datatable(sql2);
                 DataTable program = Common.datatable(sql3);

                 CreateGridColumn("学号", "学号", 150);
                 CreateGridColumn("姓名", "姓名", 150);
                 CreateGridColumn("班级", "班级", 150);


                 for (int i=0;i<=program.Rows.Count;i++)
                 {
                     if (i < program.Rows.Count)
                     {
                   //      DateTime time = Convert.ToDateTime(program.Rows[i][1].ToString()).Date;
                         CreateGridColumn(program.Rows[i][0].ToString(), program.Rows[i][0].ToString(), 150);
                     }
                     else
                     {
                        AspNet. TemplateField count = new AspNet. TemplateField();
                         GridView1.Columns.Add(count);
                     }
                 }*/
            #endregion

            DataTable data = new DataTable();
            Bind(data);
            //dt：数据源  
            GridView1.DataSource = data;
            GridView1.DataBind();

         //   output(GridView1);
        }


        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                string sql3 = "select distinct Program,Date from [Working_hoursInfor] where SySe like '%" + Session["SySe"].ToString() + "%'";
                DataTable program = Common.datatable(sql3);

                TableCellCollection header = e.Row.Cells;

                header.Clear();

                string headtxt = "学号</th><th rowspan='2'>姓名</th>";
                // headtxt += "<th colspan='"+program.Rows.Count+"'></th>";  //跨四列  
                headtxt += "<th rowspan='2'>班级</th>";
                for (int i = 0; i < program.Rows.Count; i++)
                {
                    DateTime Time = Convert.ToDateTime(program.Rows[i][1].ToString());
                    DateTime time = Time.Date;
                    headtxt += "<th>" + time.ToString("yyyy-MM-dd");
                    //  headtxt = headtxt.Substring(0, headtxt.Length - 5);  //移除掉最后一个</th>  
                }
                headtxt += "<th rowspan='2'>合计</th>";
                headtxt += "<tr>";
                for (int i = 0; i < program.Rows.Count; i++)
                {
                    headtxt += "<th>" + program.Rows[i][0].ToString() + "</th>";
                }

                headtxt += "</tr>";

                TableHeaderCell cell = new TableHeaderCell();
                cell.Attributes.Add("rowspan", "2");  //跨两行   
                cell.Text = (headtxt);
                header.Add(cell);

            }
        }


        protected void Button2_Click(object sender, EventArgs e)
        {      
            DataTable dt = new DataTable();
            Bind(dt);
         //   NPOItest.Batch_Update(dt);
                NPOIHelper.ExportByWeb(dt, GridView1.Caption, GridView1.Caption);
        }


     

    
    }
}