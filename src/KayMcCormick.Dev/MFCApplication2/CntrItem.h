
// CntrItem.h : interface of the CMFCApplication2CntrItem class
//

#pragma once

class CMFCApplication2Doc;
class CMFCApplication2View;

class CMFCApplication2CntrItem : public COleDocObjectItem
{
	DECLARE_SERIAL(CMFCApplication2CntrItem)

// Constructors
public:
	CMFCApplication2CntrItem(CMFCApplication2Doc* pContainer = nullptr);
		// Note: pContainer is allowed to be null to enable IMPLEMENT_SERIALIZE
		//  IMPLEMENT_SERIALIZE requires the class have a constructor with
		//  zero arguments.  Normally, OLE items are constructed with a
		//  non-null document pointer

// Attributes
public:
	CMFCApplication2Doc* GetDocument()
		{ return reinterpret_cast<CMFCApplication2Doc*>(COleDocObjectItem::GetDocument()); }
	CMFCApplication2View* GetActiveView()
		{ return reinterpret_cast<CMFCApplication2View*>(COleDocObjectItem::GetActiveView()); }

public:
	virtual void OnChange(OLE_NOTIFICATION wNotification, DWORD dwParam);
	virtual void OnActivate();

protected:
	virtual void OnDeactivateUI(BOOL bUndoable);
	virtual BOOL OnChangeItemPosition(const CRect& rectPos);
	virtual BOOL OnShowControlBars(CFrameWnd* pFrameWnd, BOOL bShow);

// Implementation
public:
	~CMFCApplication2CntrItem();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif
	virtual void Serialize(CArchive& ar);
};

