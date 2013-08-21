using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jhu.SpecSvc.SpectrumLib;
using Jhu.SpecSvc.IO;
using Jhu.SpecSvc.Pipeline;
using Jhu.SpecSvc.Pipeline.Formats;

namespace Jhu.SpecSvc.Web.Pipeline.Formats
{
public partial class SpectrumAsciiFormatControl : FileOutputFormatControlBase<SpectrumAsciiFormat>
{
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        if (newColumn.Items.Count == 0)
        {
            newColumn.Items.Add(new ListItem("(select column)", "new"));
            foreach (string col in AsciiConnector.ColumnNames)
            {
                newColumn.Items.Add(col);
            }
        }
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        columns.DataSource = Format.Columns;
        columns.DataBind();
    }

    protected override void OnEnabledChanged()
    {

    }

    protected override void OnUpdateForm(SpectrumAsciiFormat format)
    {
        FileType.SelectedValue = format.FileType.ToString();
        Prefix.Text = format.Prefix;
        WriteFields.Checked = format.WriteFields;
    }

    protected override void OnSaveForm(SpectrumAsciiFormat format)
    {
        format.FileType = (AsciiConnector.AsciiFileType)Enum.Parse(typeof(AsciiConnector.AsciiFileType), FileType.SelectedValue);
        format.Prefix = Prefix.Text;
        format.WriteFields = WriteFields.Checked;   
    }

    protected void columns_ItemCreated(object sender, ListViewItemEventArgs e)
    {
        if (e.Item != null && e.Item.ItemType == ListViewItemType.DataItem)
        {
            ListViewDataItem di = (ListViewDataItem)e.Item;

            if (di.DataItem != null)
            {
                string col = (string)di.DataItem;

                DropDownList column = (DropDownList)e.Item.FindControl("column");
                column.Items.Add(new ListItem("(remove column)", "remove"));
                foreach (string cc in AsciiConnector.ColumnNames)
                {
                    column.Items.Add(cc);
                }
                column.SelectedValue = col;
            }
        }
    }

    protected void newColumn_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (newColumn.SelectedValue != "new")
        {
            Format.Columns.Add(newColumn.SelectedValue);

            columns.DataBind();

            newColumn.SelectedValue = "new";
        }
    }

    protected void columns_Load(object sender, EventArgs e)
    {
        ListView lw = (ListView)sender;

        Format.Columns.Clear();

        if (Page.IsPostBack && lw.Visible)
        {
            foreach (ListViewDataItem li in lw.Items)
            {
                DropDownList col = (DropDownList)li.FindControl("column");
                if (col.SelectedValue != "remove")
                {
                    Format.Columns.Add(col.SelectedValue);
                }
            }
        }

        columns.DataBind();
    }
}
}