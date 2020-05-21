
// CntrItem.cpp : implementation of the CMFCApplication2CntrItem class
//

#include "pch.h"
#include "framework.h"
#include "MFCApplication2.h"

#include "MFCApplication2Doc.h"
#include "MFCApplication2View.h"
#include "CntrItem.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CMFCApplication2CntrItem implementation

IMPLEMENT_SERIAL(CMFCApplication2CntrItem, COleDocObjectItem, 0)

CMFCApplication2CntrItem::CMFCApplication2CntrItem(CMFCApplication2Doc* pContainer)
	: COleDocObjectItem(pContainer)
{
	// TODO: add one-time construction code here
}

CMFCApplication2CntrItem::~CMFCApplication2CntrItem()
{
	// TODO: add cleanup code here
}

void CMFCApplication2CntrItem::OnChange(OLE_NOTIFICATION nCode, DWORD dwParam)
{
	ASSERT_VALID(this);

	COleDocObjectItem::OnChange(nCode, dwParam);

	// When an item is being edited (either in-place or fully open)
	//  it sends OnChange notifications for changes in the state of the
	//  item or visual appearance of its content.

	// TODO: invalidate the item by calling UpdateAllViews
	//  (with hints appropriate to your application)

	GetDocument()->UpdateAllViews(nullptr);
		// for now just update ALL views/no hints
}

BOOL CMFCApplication2CntrItem::OnChangeItemPosition(const CRect& rectPos)
{
	ASSERT_VALID(this);

	// During in-place activation CMFCApplication2CntrItem::OnChangeItemPosition
	//  is called by the server to change the position of the in-place
	//  window.  Usually, this is a result of the data in the server
	//  document changing such that the extent has changed or as a result
	//  of in-place resizing.
	//
	// The default here is to call the base class, which will call
	//  COleDocObjectItem::SetItemRects to move the item
	//  to the new position.

	if (!COleDocObjectItem::OnChangeItemPosition(rectPos))
		return FALSE;

	// TODO: update any cache you may have of the item's rectangle/extent

	return TRUE;
}

BOOL CMFCApplication2CntrItem::OnShowControlBars(CFrameWnd* pFrameWnd, BOOL bShow)
{
	CMDIFrameWndEx* pMainFrame = DYNAMIC_DOWNCAST(CMDIFrameWndEx, pFrameWnd);

	if (pMainFrame != nullptr)
	{
		ASSERT_VALID(pMainFrame);
		return pMainFrame->OnShowPanes(bShow);
	}

	return FALSE;
}


void CMFCApplication2CntrItem::OnActivate()
{
}

void CMFCApplication2CntrItem::OnDeactivateUI(BOOL bUndoable)
{
	COleDocObjectItem::OnDeactivateUI(bUndoable);

	DWORD dwMisc = 0;
	m_lpObject->GetMiscStatus(GetDrawAspect(), &dwMisc);
	if (dwMisc & OLEMISC_INSIDEOUT)
		DoVerb(OLEIVERB_HIDE, nullptr);
}

void CMFCApplication2CntrItem::Serialize(CArchive& ar)
{
	ASSERT_VALID(this);

	// Call base class first to read in COleDocObjectItem data.
	// Since this sets up the m_pDocument pointer returned from
	//  CMFCApplication2CntrItem::GetDocument, it is a good idea to call
	//  the base class Serialize first.
	COleDocObjectItem::Serialize(ar);

	// now store/retrieve data specific to CMFCApplication2CntrItem
	if (ar.IsStoring())
	{
		// TODO: add storing code here
	}
	else
	{
		// TODO: add loading code here
	}
}


// CMFCApplication2CntrItem diagnostics

#ifdef _DEBUG
void CMFCApplication2CntrItem::AssertValid() const
{
	COleDocObjectItem::AssertValid();
}

void CMFCApplication2CntrItem::Dump(CDumpContext& dc) const
{
	COleDocObjectItem::Dump(dc);
}
#endif

