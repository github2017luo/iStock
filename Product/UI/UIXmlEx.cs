/*����FaceCat(����è)��� v1.0 https://github.com/FaceCat007/facecat.git
 *  1.����è��ʼ��-�󶴳���Ա-�Ϻ����׿Ƽ���ʼ��-����KOL-�յ� (΢�ź�:suade1984);
 *  2.���ϴ�ʼ��-�Ϻ����׿Ƽ���ʼ��-Ԭ����(΢�ź�:wx627378127);
 *  3.��̩�ڻ�Ͷ����ѯ�ܼ�/�߼��о�Ա-������(΢�ź�:18345063201)
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Drawing;
using System.Data;

namespace FaceCat {
    /// <summary>
    /// ��Ʊͼ�οؼ�Xml����
    /// </summary>
    public class UIXmlEx : FCUIXml {
        private double m_scaleFactor = 1;

        /// <summary>
        /// ��ȡ��������������
        /// </summary>
        public double ScaleFactor {
            get { return m_scaleFactor; }
            set { m_scaleFactor = value; }
        }

        /// <summary>
        /// �����ؼ�
        /// </summary>
        /// <param name="node">�ڵ�</param>
        /// <param name="type">����</param>
        /// <returns>�ؼ�</returns>
        public override FCView createControl(XmlNode node, String type) {
            FCNative native = Native;
            if (type == "barragediv") {
                return new BarrageDiv();
            }
            else if (type == "column" || type == "th") {
                return new GridColumnEx();
            }
            else if (type == "ribbonbutton") {
                return new RibbonButton();
            }
            else if (type == "indexdiv") {
                return new IndexDiv();
            }
            else if (type == "windowex") {
                return new WindowEx();
            } else if (type == "indicatorbutton") {
                return new IndicatorButton();
            }
            FCView control = base.createControl(node, type);
            if (control != null) {
                control.Font.m_fontFamily = "΢���ź�";
                if (control is FCCheckBox) {
                    (control as FCCheckBox).ButtonBackColor = FCDraw.FCCOLORS_BACKCOLOR8;
                }
            }
            return control;
        }

        /// <summary>
        /// �����˵���
        /// </summary>
        /// <param name="node">�ڵ�</param>
        /// <param name="menu">�˵�</param>
        /// <param name="parentItem">���ڵ�</param>
        protected override void createMenuItem(XmlNode node, FCMenu menu, FCMenuItem parentItem) {
            FCMenuItem item = new FCMenuItem();
            item.Native = Native;
            item.Font = new FCFont("΢���ź�", 12, false, false, false);
            setAttributesBefore(node, item);
            if (parentItem != null) {
                parentItem.addItem(item);
            }
            else {
                menu.addItem(item);
            }
            if (node.ChildNodes != null && node.ChildNodes.Count > 0) {
                foreach (XmlNode subNode in node.ChildNodes) {
                    createMenuItem(subNode, menu, item);
                }
            }
            setAttributesAfter(node, item);
            onAddControl(item, node);
        }

        /// <summary>
        /// �˳�����
        /// </summary>
        public virtual void exit() {
        }

        /// <summary>
        /// ������Excel
        /// </summary>
        /// <param name="path">·��</param>
        public static void exportToExcel(String fileName, FCGrid grid) {
            DataTable dataTable = new DataTable();
            List<FCGridColumn> columns = grid.m_columns;
            int columnsSize = columns.Count;
            for (int i = 0; i < columnsSize; i++) {
                dataTable.Columns.Add(new DataColumn(columns[i].Text));
            }
            List<FCGridRow> rows = grid.m_rows;
            int rowsSize = rows.Count;
            for (int i = 0; i < rowsSize; i++) {
                if (rows[i].Visible) {
                    DataRow dr = dataTable.NewRow();
                    for (int j = 0; j < columnsSize; j++) {
                        FCGridCell cell = grid.m_rows[i].getCell(j);
                        if (cell is FCGridStringCell) {
                            dr[j] = cell.getString();
                        }
                        else {
                            dr[j] = cell.getDouble();
                        }
                    }
                    dataTable.Rows.Add(dr);
                }
            }
            DataCenter.ExportService.ExportDataTableToExcel(dataTable, fileName);
            dataTable.Dispose();
        }

        /// <summary>
        /// ������Word
        /// </summary>
        /// <param name="fileName">�ļ���</param>
        /// <param name="html">����</param>
        public static void exportToWord(String fileName, String html) {
            DataCenter.ExportService.ExportHtmlToWord(fileName, html);
        }

        /// <summary>
        /// ������Txt
        /// </summary>
        /// <param name="fileName">�ļ���</param>
        /// <param name="grid">���ؼ�</param>
        public static void exportToTxt(String fileName, FCGrid grid) {
            StringBuilder sb = new StringBuilder();
            List<FCGridColumn> columns = grid.m_columns;
            int columnsSize = columns.Count;
            for (int i = 0; i < columnsSize; i++) {
                sb.Append(columns[i].Text);
                if (i != columnsSize - 1) {
                    sb.Append(",");
                }
            }
            List<FCGridRow> rows = grid.m_rows;
            int rowsSize = rows.Count;
            List<FCGridRow> visibleRows = new List<FCGridRow>();
            for (int i = 0; i < rowsSize; i++) {
                if (rows[i].Visible) {
                    visibleRows.Add(rows[i]);
                }
            }
            int visibleRowsSize = visibleRows.Count;
            if (visibleRowsSize > 0) {
                sb.Append("\r\n");
                for (int i = 0; i < visibleRowsSize; i++) {
                    for (int j = 0; j < columnsSize; j++) {
                        FCGridCell cell = visibleRows[i].getCell(j);
                        String cellValue = cell.getString();
                        sb.Append(cellValue);
                        if (j != columnsSize - 1) {
                            sb.Append(",");
                        }
                    }
                    if (i != visibleRowsSize - 1) {
                        sb.Append("\r\n");
                    }
                }
            }
            DataCenter.ExportService.ExportHtmlToTxt(fileName, sb.ToString());
        }

        /// <summary>
        /// ����XML
        /// </summary>
        /// <param name="xmlPath">XML��ַ</param>
        public virtual void load(String xmlPath) {
        }

        /// <summary>
        /// ��������
        /// </summary>
        public virtual void loadData() {
        }

        /// <summary>
        /// �������ųߴ�
        /// </summary>
        /// <param name="clientSize">�ͻ��˴�С</param>
        public void resetScaleSize(FCSize clientSize) {
            FCNative native = Native;
            if (native != null) {
                FCHost host = native.Host;
                FCSize nativeSize = native.DisplaySize;
                List<FCView> controls = native.getControls();
                int controlsSize = controls.Count;
                for (int i = 0; i < controlsSize; i++) {
                    FCWindowFrame frame = controls[i] as FCWindowFrame;
                    if (frame != null) {
                        WindowEx window = frame.getControls()[0] as WindowEx;
                        if (window != null && !window.AnimateMoving) {
                            FCPoint location = window.Location;
                            if (location.x < 10 || location.x > nativeSize.cx - 10) {
                                location.x = 0;
                            }
                            if (location.y < 30 || location.y > nativeSize.cy - 30) {
                                location.y = 0;
                            }
                            window.Location = location;
                        }
                    }
                }
                native.ScaleSize = new FCSize((int)(clientSize.cx * m_scaleFactor), (int)(clientSize.cy * m_scaleFactor));
                native.update();
            }
        }

        /// <summary>
        /// �Ƿ�ȷ�Ϲر�
        /// </summary>
        /// <returns>������</returns>
        public virtual int submit() {
            return 0;
        }
    }

    /// <summary>
    /// ����XML��չ
    /// </summary>
    public class WindowXmlEx : UIXmlEx {
        /// <summary>
        /// ���ÿؼ������¼�
        /// </summary>
        private FCInvokeEvent m_invokeEvent;

        protected UIXmlEx m_parent;

        /// <summary>
        /// ��ȡ�����ø�����
        /// </summary>
        public UIXmlEx Parent {
            get { return m_parent; }
            set { m_parent = value; }
        }

        protected WindowEx m_window;

        /// <summary>
        /// ��ȡ�����ô���
        /// </summary>
        public WindowEx Window {
            get { return m_window; }
        }

        /// <summary>
        /// ��ť����¼�
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="mp">����</param>
        /// <param name="button">��ť</param>
        /// <param name="click">�������</param>
        /// <param name="delta">���ֹ���ֵ</param>
        private void clickButton(object sender, FCTouchInfo touchInfo) {
            if (touchInfo.m_firstTouch && touchInfo.m_clicks == 1) {
                FCView control = sender as FCView;
                if (m_window != null && control == m_window.CloseButton) {
                    close();
                }
            }
        }

        /// <summary>
        /// �ر�
        /// </summary>
        public virtual void close() {
            m_window.invoke("close");
        }

        /// <summary>
        /// ���ٷ���
        /// </summary>
        public override void delete() {
            if (!IsDeleted) {
                if (m_window != null) {
                    m_invokeEvent = null;
                    m_window.close();
                    m_window.delete();
                    m_window = null;
                }
                base.delete();
            }
        }

        /// <summary>
        /// ���ؽ���
        /// </summary>
        public virtual void load(FCNative native, String xmlName, String windowName) {
            Native = native;
            String xmlPath = DataCenter.getAppPath() + "\\config\\" + xmlName + ".html";
            Script = new FaceCatScript(this);
            loadFile(xmlPath, null);
            m_window = findControl(windowName) as WindowEx;
            m_invokeEvent = new FCInvokeEvent(invoke);
            m_window.addEvent(m_invokeEvent, FCEventID.INVOKE);
            //ע�����¼�
            registerEvents(m_window);
        }

        /// <summary>
        /// ���ÿؼ��̷߳���
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="args">����</param>
        private void invoke(object sender, object args) {
            onInvoke(args);
        }

        /// <summary>
        /// ���ÿؼ��̷߳���
        /// </summary>
        /// <param name="args">����</param>
        public void onInvoke(object args) {
            if (args != null && args.ToString() == "close") {
                delete();
                Native.invalidate();
            }
        }

        /// <summary>
        /// ע���¼�
        /// </summary>
        /// <param name="control">�ؼ�</param>
        private void registerEvents(FCView control) {
            FCTouchEvent clickButtonEvent = new FCTouchEvent(clickButton);
            List<FCView> controls = control.getControls();
            int controlsSize = controls.Count;
            for (int i = 0; i < controlsSize; i++) {
                FCButton button = controls[i] as FCButton;
                if (button != null) {
                    button.addEvent(clickButtonEvent, FCEventID.CLICK);
                }
                registerEvents(controls[i]);
            }
        }

        /// <summary>
        /// ��ʾ
        /// </summary>
        public virtual void show() {
            m_window.animateShow(false);
            m_window.invalidate();
        }

        /// <summary>
        /// ��ʾ
        /// </summary>
        public virtual void showDialog() {
            m_window.animateShow(true);
            m_window.invalidate();
        }
    }

    /// <summary>
    /// �������չ
    /// </summary>
    public class GridColumnEx : FCGridColumn {
        /// <summary>
        /// ���캯��
        /// </summary>
        public GridColumnEx() {
            Font = new FCFont("΢���ź�", 12, false, false, false);
        }

        /// <summary>
        /// �ػ汳������
        /// </summary>
        /// <param name="paint">��ͼ����</param>
        /// <param name="clipRect">�ü�����</param>
        public override void onPaintBackground(FCPaint paint, FCRect clipRect) {
            int width = Width, height = Height;
            FCRect drawRect = new FCRect(0, 0, width, height);
            paint.fillGradientRect(FCDraw.FCCOLORS_BACKCOLOR, FCDraw.FCCOLORS_BACKCOLOR2, drawRect, 0, 90);
        }
    }
}
