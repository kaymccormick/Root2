// This MFC Samples source code demonstrates using MFC Microsoft Office Fluent User Interface
// (the "Fluent UI") and is provided only as referential material to supplement the
// Microsoft Foundation Classes Reference and related electronic documentation
// included with the MFC C++ library software.
// License terms to copy, use or distribute the Fluent UI are available separately.
// To learn more about our Fluent UI licensing program, please visit
// https://go.microsoft.com/fwlink/?LinkId=238214.
//
// Copyright (C) Microsoft Corporation
// All rights reserved.

// MainFrm.cpp : implementation of the CMainFrame class
//

#include "pch.h"
#include "framework.h"
#include "MFCApplication1.h"
#include "WpfPage.h"
#include "MainFrm.h"

#include "Win32HostingWPFPage.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

// CMainFrame

IMPLEMENT_DYNCREATE(CMainFrame, CFrameWndEx)

BEGIN_MESSAGE_MAP(CMainFrame, CFrameWndEx)
	ON_WM_CREATE()
END_MESSAGE_MAP()

// CMainFrame construction/destruction

CMainFrame::CMainFrame() noexcept
{
	// TODO: add member initialization code here
}

CMainFrame::~CMainFrame()
{
}

int CMainFrame::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	if (CFrameWndEx::OnCreate(lpCreateStruct) == -1)
		return -1;

	BOOL bNameValid;

	m_wndRibbonBar.Create(this);
	m_wndRibbonBar.LoadFromResource(IDR_RIBBON);

	if (!m_wndStatusBar.Create(this))
	{
		TRACE0("Failed to create status bar\n");
		return -1;      // fail to create
	}

	CString strTitlePane1;
	CString strTitlePane2;
	bNameValid = strTitlePane1.LoadString(IDS_STATUS_PANE1);
	ASSERT(bNameValid);
	bNameValid = strTitlePane2.LoadString(IDS_STATUS_PANE2);
	ASSERT(bNameValid);
	m_wndStatusBar.AddElement(new CMFCRibbonStatusBarPane(ID_STATUSBAR_PANE1, strTitlePane1, TRUE), strTitlePane1);
	m_wndStatusBar.AddExtendedElement(new CMFCRibbonStatusBarPane(ID_STATUSBAR_PANE2, strTitlePane2, TRUE), strTitlePane2);

	// enable Visual Studio 2005 style docking window behavior
	CDockingManager::SetDockingMode(DT_SMART);
	// enable Visual Studio 2005 style docking window auto-hide behavior
	EnableAutoHidePanes(CBRS_ALIGN_ANY);

	tagRECT _rect;
	this->GetClientRect(&_rect);
	GetHwnd(m_hWnd, 0, 150, _rect.right, _rect.bottom);
	
	
	return 0;
}

WPFPageHost::WPFPageHost() {}
HWND GetHwnd(HWND parent, int x, int y, int width, int height)
{
	System::Windows::Interop::HwndSourceParameters^ sourceParams = gcnew System::Windows::Interop::HwndSourceParameters(
		"hi" // NAME
	);
	sourceParams->PositionX = x;
	sourceParams->PositionY = y;
	sourceParams->Height = height;
	sourceParams->Width = width;
	sourceParams->ParentWindow = IntPtr(parent);
	sourceParams->WindowStyle = WS_VISIBLE | WS_CHILD; // style
	System::Windows::Interop::HwndSource^ source = gcnew System::Windows::Interop::HwndSource(*sourceParams);
	WPFPage^ myPage = gcnew WPFPage(width, height);
	//Assign a reference to the WPF page and a set of UI properties to a set of static properties in a class
	//that is designed for that purpose.
	WPFPageHost::hostedPage = myPage;
	WPFPageHost::initBackBrush = Brushes::Transparent;
	// WPFPageHost::initFontFamily = myPage->DefaultFontFamily;
	// WPFPageHost::initFontSize = myPage->DefaultFontSize;
	// WPFPageHost::initFontStyle = myPage->DefaultFontStyle;
	// WPFPageHost::initFontWeight = myPage->DefaultFontWeight;
	// WPFPageHost::initForeBrush = myPage->DefaultForeBrush;
	// myPage->OnButtonClicked += gcnew WPFPage::ButtonClickHandler(WPFButtonClicked);
	source->RootVisual = myPage;
	return (HWND)source->Handle.ToPointer();
}


BOOL CMainFrame::PreCreateWindow(CREATESTRUCT& cs)
{
	if( !CFrameWndEx::PreCreateWindow(cs) )
		return FALSE;
	// TODO: Modify the Window class or styles here by modifying
	//  the CREATESTRUCT cs

	return TRUE;
}

// CMainFrame diagnostics

#ifdef _DEBUG
void CMainFrame::AssertValid() const
{
	CFrameWndEx::AssertValid();
}

void CMainFrame::Dump(CDumpContext& dc) const
{
	CFrameWndEx::Dump(dc);
}
#endif //_DEBUG


// CMainFrame message handlers

