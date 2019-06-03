/*����FaceCat(����è)��� v1.0 https://github.com/FaceCat007/facecat.git
 *  1.����è��ʼ��-�󶴳���Ա-�Ϻ����׿Ƽ���ʼ��-����KOL-�յ� (΢�ź�:suade1984);
 *  2.���ϴ�ʼ��-�Ϻ����׿Ƽ���ʼ��-Ԭ����(΢�ź�:wx627378127);
 *  3.��̩�ڻ�Ͷ����ѯ�ܼ�/�߼��о�Ա-������(΢�ź�:18345063201)
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace FaceCat {
    /// <summary>
    /// ���̾���
    /// </summary>
    public class SearchDiv : FCMenu {
        /// <summary>
        /// �������̾���
        /// </summary>
        public SearchDiv() {
            m_gridCellClickEvent = new FCGridCellTouchEvent(gridCellClick);
            m_gridKeyDownEvent = new FCKeyEvent(GridKeyDown);
            m_textBoxInputChangedEvent = new FCEvent(textBoxInputChanged);
            m_textBoxKeyDownEvent = new FCKeyEvent(textBoxKeyDown);
            BackColor = FCColor.None;
            IsWindow = true;
        }

        /// <summary>
        /// ���ؼ�
        /// </summary>
        private FCGrid m_grid;

        /// <summary>
        /// ���Ԫ�����¼�
        /// </summary>
        private FCGridCellTouchEvent m_gridCellClickEvent;

        /// <summary>
        /// �������¼�
        /// </summary>
        private FCKeyEvent m_gridKeyDownEvent;

        /// <summary>
        /// �ı�������ı��¼�
        /// </summary>
        private FCEvent m_textBoxInputChangedEvent;

        /// <summary>
        /// �ı�������¼�
        /// </summary>
        private FCKeyEvent m_textBoxKeyDownEvent;

        private MainFrame m_mainFrame;

        /// <summary>
        /// ��ȡ�����ù�Ʊ�ؼ�
        /// </summary>
        public MainFrame MainFrame {
            get { return m_mainFrame; }
            set { m_mainFrame = value; }
        }

        private FCTextBox m_searchTextBox;

        /// <summary>
        /// ��ȡ�����ò����ı���
        /// </summary>
        public FCTextBox SearchTextBox {
            get { return m_searchTextBox; }
            set { m_searchTextBox = value; }
        }

        /// <summary>
        /// ���ٷ���
        /// </summary>
        public override void delete() {
            if (!IsDeleted) {
                if (m_grid != null) {
                    m_grid.removeEvent(m_gridCellClickEvent, FCEventID.GRIDCELLCLICK);
                    m_grid.removeEvent(m_gridKeyDownEvent, FCEventID.KEYDOWN);
                }
                if (m_searchTextBox != null) {
                    if (m_textBoxInputChangedEvent != null) {
                        m_searchTextBox.removeEvent(m_textBoxInputChangedEvent, FCEventID.TEXTCHANGED);
                        m_textBoxInputChangedEvent = null;
                    }
                    if (m_textBoxKeyDownEvent != null) {
                        m_searchTextBox.removeEvent(m_textBoxKeyDownEvent, FCEventID.KEYDOWN);
                        m_textBoxKeyDownEvent = null;
                    }
                }
            }
            base.delete();
        }

        /// <summary>
        /// ���˲���
        /// </summary>
        public void filterSearch() {
            String sText = m_searchTextBox.Text.ToUpper();
            m_grid.beginUpdate();
            m_grid.clearRows();
            int row = 0;
            CList<Security> securities = SecurityService.FilterCode(sText);
            if (securities != null) {
                int rowCount = securities.size();
                for (int i = 0; i < rowCount; i++) {
                    Security security = securities.get(i);
                    FCGridRow gridRow = new FCGridRow();
                    m_grid.addRow(gridRow);
                    gridRow.addCell(0, new FCGridStringCell(security.m_code));
                    gridRow.addCell(1, new FCGridStringCell(security.m_name));
                    row++;
                }
            }
            securities.delete();
            m_grid.endUpdate();
        }

        /// <summary>
        /// ���Ԫ�����¼�
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="cell">��Ԫ��</param>
        /// <param name="mp">����</param>
        /// <param name="button">��ť</param>
        /// <param name="clicks">�������</param>
        /// <param name="delta">����ֵ</param>
        private void gridCellClick(object sender, FCGridCell cell, FCTouchInfo touchInfo) {
            if (touchInfo.m_firstTouch && touchInfo.m_clicks == 2) {
                onSelectRow();
            }
        }

        /// <summary>
        /// �������¼�
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="key">����</param>
        private void GridKeyDown(object sender, char key) {
            if (key == 13) {
                onSelectRow();
            }
        }

        /// <summary>
        /// ��ӿؼ�����
        /// </summary>
        public override void onLoad() {
            base.onLoad();
            if (m_grid == null) {
                m_grid = new FCGrid();
                m_grid.AutoEllipsis = true;
                m_grid.GridLineColor = FCColor.None;
                m_grid.Size = new FCSize(240, 200);
                m_grid.addEvent(m_gridCellClickEvent, FCEventID.GRIDCELLCLICK);
                m_grid.addEvent(m_gridKeyDownEvent, FCEventID.KEYDOWN);
                addControl(m_grid);
                m_grid.beginUpdate();
                //�����
                FCGridColumn securityCodeColumn = new FCGridColumn("��Ʊ����");
                securityCodeColumn.BackColor = FCDraw.FCCOLORS_BACKCOLOR;
                securityCodeColumn.BorderColor = FCColor.None;
                securityCodeColumn.Font = new FCFont("Simsun", 14, true, false, false);
                securityCodeColumn.TextColor = FCDraw.FCCOLORS_FORECOLOR;
                securityCodeColumn.TextAlign = FCContentAlignment.MiddleLeft;
                securityCodeColumn.Width = 120;
                m_grid.addColumn(securityCodeColumn);
                FCGridColumn securityNameColumn = new FCGridColumn("��Ʊ����");
                securityNameColumn.BackColor = FCDraw.FCCOLORS_BACKCOLOR;
                securityNameColumn.BorderColor = FCColor.None;
                securityNameColumn.Font = new FCFont("Simsun", 14, true, false, false);
                securityNameColumn.TextColor = FCDraw.FCCOLORS_FORECOLOR;
                securityNameColumn.TextAlign = FCContentAlignment.MiddleLeft;
                securityNameColumn.Width = 110;
                m_grid.addColumn(securityNameColumn);
                m_grid.endUpdate();
            }
            if (m_searchTextBox == null) {
                m_searchTextBox = new FCTextBox();
                m_searchTextBox.Location = new FCPoint(0, 200);
                m_searchTextBox.Size = new FCSize(240, 20);
                m_searchTextBox.Font = new FCFont("SimSun", 16, true, false, false);
                m_searchTextBox.addEvent(m_textBoxInputChangedEvent, FCEventID.TEXTCHANGED);
                m_searchTextBox.addEvent(m_textBoxKeyDownEvent, FCEventID.KEYDOWN);
                addControl(m_searchTextBox);
            }
        }

        /// <summary>
        /// ѡ���з���
        /// </summary>
        private void onSelectRow() {
            List<FCGridRow> rows = m_grid.SelectedRows;
            if (rows != null && rows.Count > 0) {
                FCGridRow selectedRow = rows[0];
                Security security = new Security();
                SecurityService.getSecurityByCode(selectedRow.getCell(0).Text, ref security);
                m_mainFrame.findControl("txtCode").Text = security.m_code;
                Visible = false;
                invalidate();
            }
        }

        /// <summary>
        /// ���̰��·���
        /// </summary>
        /// <param name="sender">�ؼ�</param>
        /// <param name="key">����</param>
        /// <returns>�Ƿ���</returns>
        private void textBoxKeyDown(object sender, char key) {
            if (key == 13) {
                onSelectRow();
            }
            else if (key == 38 || key == 40) {
                onKeyDown(key);
            }
        }

        /// <summary>
        /// �ı�������
        /// </summary>
        /// <param name="sender">�ؼ�</param>
        private void textBoxInputChanged(object sender) {
            FCTextBox control = sender as FCTextBox;
            SearchTextBox = control;
            filterSearch();
            String text = control.Text;
            if (text != null && text.Length == 0) {
                Visible = false;
            }
            invalidate();
        }

        /// <summary>
        /// ���̷���
        /// </summary>
        /// <param name="key">����</param>
        public override void onKeyDown(char key) {
            base.onKeyDown(key);
            if (key == 13) {
                onSelectRow();
            }
            else if (key == 38 || key == 40) {
                m_grid.onKeyDown(key);
            }
        }
    }
}
