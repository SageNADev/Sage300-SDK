// The MIT License (MIT) 
// Copyright (c) 1994-2018 The Sage Group plc or its licensors.  All rights reserved.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of 
// this software and associated documentation files (the "Software"), to deal in 
// the Software without restriction, including without limitation the rights to use, 
// copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the 
// Software, and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

function writeActxViewer(sViewerVer, sProductLang, sPreferredViewingLang, bDrillDown, bExport, bDisplayGroupTree, 
						bGroupTree, bAnimation, bPrint, bRefresh, bSearch, 
						bZoom, bSearchExpert, bSelectExpert, sParamVer) {
	document.write("<OBJECT ID=\"CRViewer\"");
	document.write("CLASSID=\"CLSID:C0A870C3-66BB-4106-9A25-60A26F3C1DA8\"");
	document.write("WIDTH=\"100%\" HEIGHT=\"99%\"");
	document.write("CODEBASE=\"" + gPath + viewerPath + "ActiveXControls/ActiveXViewer.cab#Version=" + sViewerVer + "\">");
	document.write("<PARAM NAME=\"LocaleID\" VALUE=\"" + sProductLang + "\">");
	document.write("<PARAM NAME=\"PreferredViewingLocaleID\" VALUE=\"" + sPreferredViewingLang + "\">");
	document.write("<PARAM NAME=\"EnableDrillDown\" VALUE=" + bDrillDown + ">");
	document.write("<PARAM NAME=\"EnableExportButton\" VALUE=" + bExport + ">");
	document.write("<PARAM NAME=\"DisplayGroupTree\" VALUE=" + bDisplayGroupTree + ">");
	
	document.write("<PARAM NAME=\"EnableGroupTree\" VALUE=" + bGroupTree +">");
	document.write("<PARAM NAME=\"EnableAnimationControl\" VALUE=" + bAnimation + ">");
	document.write("<PARAM NAME=\"EnablePrintButton\" VALUE=" + bPrint + ">");
	document.write("<PARAM NAME=\"EnableRefreshButton\" VALUE=" + bRefresh + ">");
	document.write("<PARAM NAME=\"EnableSearchControl\" VALUE=" + bSearch + ">");
	
	document.write("<PARAM NAME=\"EnableZoomControl\" VALUE=" + bZoom + ">");
	document.write("<PARAM NAME=\"EnableSearchExpertButton\" VALUE=" + bSearchExpert + ">");
	document.write("<PARAM NAME=\"EnableSelectExpertButton\" VALUE=" + bSelectExpert + ">");
	document.write("</OBJECT>");

	document.write("<OBJECT ID=\"ReportSource\"");
	document.write("CLASSID=\"CLSID:C05C1BE9-3285-4ED8-B47E-8F408534E89D\"");
	document.write("HEIGHT=\"1%\" WIDTH=\"1%\"");
	document.write("CODEBASE=\"" + gPath + viewerPath + "ActiveXControls/ActiveXViewer.cab#Version=" + sViewerVer + "\">");
	document.write("</OBJECT>");

	document.write("<OBJECT ID=\"ViewHelp\"");
	document.write("CLASSID=\"CLSID:C02176CF-8629-4AF6-8F96-00D2DAA4EFB2\"");
	document.write("HEIGHT=\"1%\" WIDTH=\"1%\"");
	document.write("CODEBASE=\"" + gPath + viewerPath + "ActiveXControls/ActiveXViewer.cab#Version=" + sViewerVer + "\">");
	document.write("</OBJECT>");	
}

