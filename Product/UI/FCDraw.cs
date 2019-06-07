/*����FaceCat(����è)��� v1.0 https://github.com/FaceCat007/facecat.git
 *  1.����è��ʼ��-�󶴳���Ա-�Ϻ����׿Ƽ���ʼ��-����KOL-�յ� (΢�ź�:suade1984);
 *  2.���ϴ�ʼ��-�Ϻ����׿Ƽ���ʼ��-Ԭ����(΢�ź�:wx627378127);
 *  3.��̩�ڻ�Ͷ����ѯ�ܼ�/�߼��о�Ա-������(΢�ź�:18345063201);
 *  4.���ϴ�ʼ��-Ф����(΢�ź�:xiaotianlong_luu);
 */

using System;
using System.Collections.Generic;
using System.Text;
using FaceCat;

namespace FaceCat {
    /// <summary>
    /// ��ͼ��
    /// </summary>
    public class FCDraw {
        /// <summary>
        /// �û��Զ���ɫ
        /// </summary>
        public const long FCCOLORS_USERCOLOR = -200000000100;

        /// <summary>
        /// ����ɫ
        /// </summary>
        public const long FCCOLORS_BACKCOLOR = FCCOLORS_USERCOLOR - 1;

        /// <summary>
        /// ����ɫ2
        /// </summary>
        public const long FCCOLORS_BACKCOLOR2 = FCCOLORS_USERCOLOR - 2;

        /// <summary>
        /// ����ɫ3
        /// </summary>
        public const long FCCOLORS_BACKCOLOR3 = FCCOLORS_USERCOLOR - 3;

        /// <summary>
        /// ����ɫ4
        /// </summary>
        public const long FCCOLORS_BACKCOLOR4 = FCCOLORS_USERCOLOR - 4;

        /// <summary>
        /// ����ɫ5
        /// </summary>
        public const long FCCOLORS_BACKCOLOR5 = FCCOLORS_USERCOLOR - 5;

        /// <summary>
        /// ����ɫ6
        /// </summary>
        public const long FCCOLORS_BACKCOLOR6 = FCCOLORS_USERCOLOR - 6;

        /// <summary>
        /// ����ɫ7
        /// </summary>
        public const long FCCOLORS_BACKCOLOR7 = FCCOLORS_USERCOLOR - 7;

        /// <summary>
        /// ����ɫ8
        /// </summary>
        public const long FCCOLORS_BACKCOLOR8 = FCCOLORS_USERCOLOR - 8;

        /// <summary>
        /// ����ɫ9
        /// </summary>
        public const long FCCOLORS_BACKCOLOR9 = FCCOLORS_USERCOLOR - 9;

        /// <summary>
        /// ����ɫ10
        /// </summary>
        public const long FCCOLORS_BACKCOLOR10 = FCCOLORS_USERCOLOR - 10;

        /// <summary>
        /// ����ɫ11
        /// </summary>
        public const long FCCOLORS_BACKCOLOR11 = FCCOLORS_USERCOLOR - 11;

        /// <summary>
        /// ����ɫ12
        /// </summary>
        public const long FCCOLORS_BACKCOLOR12 = FCCOLORS_USERCOLOR - 12;

        /// <summary>
        /// ǰ��ɫ
        /// </summary>
        public const long FCCOLORS_FORECOLOR = FCCOLORS_USERCOLOR - 100;

        /// <summary>
        /// ǰ��ɫ2
        /// </summary>
        public const long FCCOLORS_FORECOLOR2 = FCCOLORS_USERCOLOR - 101;

        /// <summary>
        /// ǰ��ɫ3
        /// </summary>
        public const long FCCOLORS_FORECOLOR3 = FCCOLORS_USERCOLOR - 102;

        /// <summary>
        /// ǰ��ɫ4
        /// </summary>
        public const long FCCOLORS_FORECOLOR4 = FCCOLORS_USERCOLOR - 103;

        /// <summary>
        /// ǰ��ɫ5
        /// </summary>
        public const long FCCOLORS_FORECOLOR5 = FCCOLORS_USERCOLOR - 104;

        /// <summary>
        /// ǰ��ɫ6
        /// </summary>
        public const long FCCOLORS_FORECOLOR6 = FCCOLORS_USERCOLOR - 105;

        /// <summary>
        /// ǰ��ɫ9
        /// </summary>
        public const long FCCOLORS_FORECOLOR9 = FCCOLORS_USERCOLOR - 108;

        /// <summary>
        /// ǰ��ɫ11
        /// </summary>
        public const long FCCOLORS_FORECOLOR11 = FCCOLORS_USERCOLOR - 110;

        /// <summary>
        /// �ߵ���ɫ
        /// </summary>
        public const long FCCOLORS_LINECOLOR = FCCOLORS_USERCOLOR - 200;

        /// <summary>
        /// �ߵ���ɫ2
        /// </summary>
        public const long FCCOLORS_LINECOLOR2 = FCCOLORS_USERCOLOR - 201;

        /// <summary>
        /// �ߵ���ɫ3
        /// </summary>
        public const long FCCOLORS_LINECOLOR3 = FCCOLORS_USERCOLOR - 202;

        /// <summary>
        /// �ߵ���ɫ4
        /// </summary>
        public const long FCCOLORS_LINECOLOR4 = FCCOLORS_USERCOLOR - 203;

        /// <summary>
        /// ����ɫ
        /// </summary>
        public const long FCCOLORS_UPCOLOR = FCCOLORS_USERCOLOR - 301;

        /// <summary>
        /// �µ�ɫ
        /// </summary>
        public const long FCCOLORS_DOWNCOLOR = FCCOLORS_USERCOLOR - 302;

        /// <summary>
        /// ���ǲ���ɫ
        /// </summary>
        public const long FCCOLORS_MIDCOLOR = FCCOLORS_USERCOLOR - 305;

        /// <summary>
        /// ѡ������ɫ
        /// </summary>
        public const long FCCOLORS_SELECTEDROWCOLOR = FCCOLORS_USERCOLOR - 400;

        /// <summary>
        /// ��ͣ����ɫ
        /// </summary>
        public const long FCCOLORS_HOVEREDROWCOLOR = FCCOLORS_USERCOLOR - 401;

        /// <summary>
        /// ��������ɫ
        /// </summary>
        public const long FCCOLORS_ALTERNATEROWCOLOR = FCCOLORS_USERCOLOR - 402;

        /// <summary>
        /// ����ǰ��ɫ
        /// </summary>
        public const long FCCOLORS_WINDOWFORECOLOR = FCCOLORS_USERCOLOR - 500;

        /// <summary>
        /// ���屳��ɫ
        /// </summary>
        public const long FCCOLORS_WINDOWBACKCOLOR = FCCOLORS_USERCOLOR - 501;

        /// <summary>
        /// ���屳��ɫ2
        /// </summary>
        public const long FCCOLORS_WINDOWBACKCOLOR2 = FCCOLORS_USERCOLOR - 502;

        /// <summary>
        /// ���屳��ɫ3
        /// </summary>
        public const long FCCOLORS_WINDOWBACKCOLOR3 = FCCOLORS_USERCOLOR - 503;

        /// <summary>
        /// ����ʮ������ɫ
        /// </summary>
        public const long FCCOLORS_WINDOWCROSSLINECOLOR = FCCOLORS_USERCOLOR - 504;

        /// <summary>
        /// ����ʮ������ɫ2
        /// </summary>
        public const long FCCOLORS_WINDOWCROSSLINECOLOR2 = FCCOLORS_USERCOLOR - 505;

        /// <summary>
        /// �������ݱ���ɫ
        /// </summary>
        public const long FCCOLORS_WINDOWCONTENTBACKCOLOR = FCCOLORS_USERCOLOR - 506;

        public const long FCCOLORS_EXCOLOR1 = FCCOLORS_USERCOLOR - 600;
        public const long FCCOLORS_EXCOLOR2 = FCCOLORS_USERCOLOR - 601;
        public const long FCCOLORS_EXCOLOR3 = FCCOLORS_USERCOLOR - 602;
        public const long FCCOLORS_EXCOLOR4 = FCCOLORS_USERCOLOR - 603;
        public const long FCCOLORS_EXCOLOR5 = FCCOLORS_USERCOLOR - 604;
        public const long FCCOLORS_EXCOLOR6 = FCCOLORS_USERCOLOR - 605;
        public const long FCCOLORS_EXCOLOR7 = FCCOLORS_USERCOLOR - 606;
        public const long FCCOLORS_EXCOLOR8 = FCCOLORS_USERCOLOR - 607;
        public const long FCCOLORS_EXCOLOR9 = FCCOLORS_USERCOLOR - 608;
        public const long FCCOLORS_EXCOLOR10 = FCCOLORS_USERCOLOR - 609;

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="paint">��ͼ����</param>
        /// <param name="text">����</param>
        /// <param name="dwPenColor">��ɫ</param>
        /// <param name="font">����</param>
        /// <param name="x">������</param>
        /// <param name="y">������</param>
        public static FCSize drawText(FCPaint paint, String text, long dwPenColor, FCFont font, int x, int y) {
            FCSize tSize = paint.textSize(text, font);
            FCRect tRect = new FCRect(x, y, x + tSize.cx, y + tSize.cy);
            paint.drawText(text, dwPenColor, font, tRect);
            return tSize;
        }

        /// <summary>
        /// �������»��ߵ�����
        /// </summary>
        /// <param name="paint">��ͼ����</param>
        /// <param name="value">ֵ</param>
        /// <param name="digit">����С��λ��</param>
        /// <param name="font">����</param>
        /// <param name="fontColor">������ɫ</param>
        /// <param name="zeroAsEmpty">0�Ƿ�Ϊ��</param>
        /// <param name="x">������</param>
        /// <param name="y">������</param>
        /// <returns>���Ƶĺ�����</returns>
        public static int drawUnderLineNum(FCPaint paint, double value, int digit, FCFont font, long fontColor, bool zeroAsEmpty, int x, int y) {
            if (zeroAsEmpty && value == 0) {
                String text = "-";
                FCSize size = paint.textSize(text, font);
                FCDraw.drawText(paint, text, fontColor, font, x, y);
                return size.cx;
            } else {
                String[] nbs = FCStr.getValueByDigit(value, digit).Split(new String[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                if (nbs.Length == 1) {
                    FCSize size = paint.textSize(nbs[0], font);
                    FCDraw.drawText(paint, nbs[0], fontColor, font, x, y);
                    return size.cx;
                } else {
                    FCSize decimalSize = paint.textSize(nbs[0], font);
                    FCSize size = paint.textSize(nbs[1], font);
                    FCDraw.drawText(paint, nbs[0], fontColor, font, x, y);
                    FCDraw.drawText(paint, nbs[1], fontColor, font, x
                        + decimalSize.cx, y);
                    paint.drawLine(fontColor, 1, 0, x
                        + decimalSize.cx, y + decimalSize.cy,
                        x + decimalSize.cx + size.cx, y + decimalSize.cy);
                    return decimalSize.cx + size.cx;
                }
            }
        }

        /// <summary>
        /// ��ȡ��ɫ������ɫ
        /// </summary>
        /// <param name="color">��ɫ</param>
        /// <returns>�µ���ɫ</returns>
        public static long getBlackColor(long color) {
            if (color > FCCOLORS_USERCOLOR) {
                if (color == FCColor.Back) {
                    color = FCColor.argb(19, 21, 26);
                } else if (color == FCColor.Border) {
                    color = FCColor.argb(50, 105, 217);
                } else if (color == FCColor.Text) {
                    color = FCColor.argb(255, 255, 255);
                } else if (color == FCColor.DisabledBack) {
                    color = FCColor.argb(25, 255, 255, 255);
                } else if (color == FCColor.DisabledText) {
                    color = 3289650;
                } else if (color == FCColor.Hovered) {
                    color = FCColor.argb(150, 43, 138, 195);
                } else if (color == FCColor.Pushed) {
                    color = FCColor.argb(100, 43, 138, 195);
                }
            }
            if (color == FCCOLORS_BACKCOLOR) {
                color = FCColor.argb(180, 43, 138, 195);
            } else if (color == FCCOLORS_BACKCOLOR2) {
                color = FCColor.argb(130, 43, 138, 195);
            } else if (color == FCCOLORS_BACKCOLOR3) {
                color = FCColor.argb(25, 100, 100, 100);
            } else if (color == FCCOLORS_BACKCOLOR4) {
                color = FCColor.argb(255, 255, 255);
            } else if (color == FCCOLORS_BACKCOLOR5) {
                color = FCColor.argb(25, 255, 255, 255);
            } else if (color == FCCOLORS_BACKCOLOR6) {
                color = FCColor.argb(25, 0, 0, 0);
            } else if (color == FCCOLORS_BACKCOLOR7) {
                color = FCColor.argb(200, 255, 0, 0);
            } else if (color == FCCOLORS_BACKCOLOR8) {
                color = FCColor.argb(200, 0, 0, 0);
            } else if (color == FCCOLORS_BACKCOLOR9) {
                color = FCColor.argb(9, 30, 42);
            } else if (color == FCCOLORS_FORECOLOR) {
                color = FCColor.argb(255, 255, 255);
            } else if (color == FCCOLORS_FORECOLOR2) {
                color = FCColor.argb(0, 255, 255);
            } else if (color == FCCOLORS_FORECOLOR3) {
                color = FCColor.argb(255, 200, 0);
            } else if (color == FCCOLORS_FORECOLOR4) {
                color = FCColor.argb(255, 255, 255);
            } else if (color == FCCOLORS_LINECOLOR) {
                color = FCColor.argb(100, 100, 100);
            } else if (color == FCCOLORS_LINECOLOR2) {
                color = FCColor.argb(0, 105, 217);
            } else if (color == FCCOLORS_LINECOLOR3) {
                color = FCColor.argb(5, 255, 255, 255);
            } else if (color == FCCOLORS_LINECOLOR4) {
                color = FCColor.argb(255, 80, 80);
            } else if (color == FCCOLORS_MIDCOLOR) {
                color = FCColor.argb(255, 255, 255);
            } else if (color == FCCOLORS_UPCOLOR) {
                color = FCColor.argb(255, 80, 80);
            } else if (color == FCCOLORS_DOWNCOLOR) {
                color = FCColor.argb(80, 255, 80);
            } else if (color == FCCOLORS_SELECTEDROWCOLOR) {
                color = FCColor.argb(200, 43, 138, 195);
            } else if (color == FCCOLORS_HOVEREDROWCOLOR) {
                color = FCColor.argb(100, 43, 138, 195);
            } else if (color == FCDraw.FCCOLORS_WINDOWBACKCOLOR) {
                color = FCColor.argb(255, 50, 50, 50);
            } else if (color == FCDraw.FCCOLORS_WINDOWBACKCOLOR2) {
                color = FCColor.argb(100, 43, 138, 195);
            } else if (color == FCDraw.FCCOLORS_WINDOWBACKCOLOR3) {
                color = FCColor.argb(230, 43, 138, 195);
            } else if (color == FCDraw.FCCOLORS_WINDOWCROSSLINECOLOR) {
                color = FCColor.argb(100, 100, 100);
            } else if (color == FCDraw.FCCOLORS_WINDOWCROSSLINECOLOR2) {
                color = FCColor.argb(10, 255, 255, 255);
            } else if (color == FCCOLORS_WINDOWCONTENTBACKCOLOR) {
                color = FCColor.argb(235, 9, 30, 42);
            } else if (color == FCCOLORS_EXCOLOR1) {
                color = FCColor.argb(100, 50, 105, 217);
            } else if (color == FCCOLORS_EXCOLOR2) {
                color = FCColor.argb(43, 138, 195);
            } else if (color == FCCOLORS_EXCOLOR3) {
                color = FCColor.argb(50, 50, 105, 217);
            } else if (color == FCCOLORS_EXCOLOR4) {
                color = FCColor.argb(50, 255, 255, 255);
            } else if (color == FCCOLORS_EXCOLOR5) {
                color = FCColor.argb(43, 138, 195);
            } else if (color == FCCOLORS_EXCOLOR6) {
                color = FCColor.argb(100, 50, 105, 217);
            } else if (color == FCCOLORS_EXCOLOR7) {
                color = FCColor.argb(255, 255, 255);
            } else if (color == FCCOLORS_EXCOLOR8) {
                color = FCColor.argb(100, 255, 255, 255);
            } else if (color == FCCOLORS_EXCOLOR9) {
                color = FCColor.argb(50, 105, 217);
            } else if (color == FCCOLORS_EXCOLOR10) {
                color = FCColor.argb(9, 30, 42);
            }
            return color;
        }

        /// <summary>
        /// ���ݼ۸��ȡ��ɫ
        /// </summary>
        /// <param name="price">�۸�</param>
        /// <param name="comparePrice">�Ƚϼ۸�</param>
        /// <returns>��ɫ</returns>
        public static long getPriceColor(double price, double comparePrice) {
            if (price != 0) {
                if (price > comparePrice) {
                    return FCDraw.FCCOLORS_UPCOLOR;
                } else if (price < comparePrice) {
                    return FCDraw.FCCOLORS_DOWNCOLOR;
                }
            }
            return FCColor.argb(255, 255, 255);
        }
    }
}
