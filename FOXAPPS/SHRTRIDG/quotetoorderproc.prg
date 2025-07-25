PROCEDURE get_company_info 
	PARAMETERS 	cCOID
		
		PUBLIC slineone, slinetwo, scity, sstate, szip, scountry, sarea, sphone, sattn
		PUBLIC blineone, blinetwo, bcity, bstate, bzip, bcountry, barea, bphone, battn
 
 		 SELECT 1
 		 LOCATE FOR co_id = ALLTRIM(cCOID)
		 Store company.co_id to cono
		 store company.co_name to coname

		  slineone=""
		  slinetwo=""
		  scity=""
		  sstate=""
		  szip=""
		  scountry=""
		  sarea=""
		  sphone=""
		  sattn=""
		  blineone=""
		  blinetwo=""
		  bcity=""
		  bstate=""
		  bzip=""
		  bcountry=""
		  barea=""
		  bphone=""
		  battn=""

		  store company.co_id to cono
		  store company.s_addr1 to slineone
		  store company.s_addr2 to slinetwo
		  store company.s_city to scity
		  store company.s_state to sstate
		  store company.s_zip to szip
		  store company.s_country to scountry
		  store company.s_areacode to sarea
		  store company.s_phone to sphone
		  store company.s_attn to sattn

		  store company.b_addr1 to blineone
		  store company.b_addr2 to blinetwo
		  store company.b_city to bcity
		  store company.b_state to bstate
		  store company.b_zip to bzip
		  store company.b_country to bcountry
		  store company.b_areacode to barea
		  store company.b_phone to bphone
		  store company.b_attn to battn

ENDPROC

PROCEDURE quote_populate_ordrstat
	PARAMETERS orderno

	USE quote_header SHARED 
	LOCATE FOR ALLTRIM(quote_no) == ALLTRIM(quoteno) AND NOT DELETED()
	Found = FOUND()
	STORE ship_via TO shipvia
	STORE from TO takenby
	STORE frst_name TO firstname
	STORE last_name TO lastname
	STORE area_code TO areacode
	STORE phone_no TO phoneno
	STORE sub_total TO subtotal
	STORE ref_no TO po_num
	STORE freight TO frght
	STORE taxes TO tx
	STORE total TO ttl	
	STORE tax_rate TO tx_rate
	REPLACE new_order WITH orderno
	
	PATH = ""
	DO ORDRPATH.PRG
	
	USE PATH + "ORDRSTAT" SHARED 
	LOCATE FOR ALLTRIM(order_no) == ALLTRIM(orderno) AND !DELETED()
	FOUND = FOUND()
	
*!*		SELECT ordrstat
*!*		SCATTER NAME oOrdrstat BLANK
*!*		oOrdrstat.co_id = coid
*!*		oOrdrstat.co_name = coname
*!*		oOrdrstat.order_no = orderno
	*oOrdrstat.ord_dat = 
	*oOrdrstat.ship_date =
	Replace ship_via WITH  shipvia
	Replace taken_by WITH LEFT(takenby, 1) + SUBSTR(takenby, AT(" ", takenby) + 1, 1)
	REPLACE po_no WITH po_num
	REPLACE placed_by WITH ALLTRIM(firstname) + " " + ALLTRIM(lastname)
	*oOrdrstat.pb_title = 
	REPLACE pb_area WITH areacode
	REPLACE pb_phone WITH phoneno
	*oOrdrstat.pb_ext = 
	*oOrdrstat.order_trms = 
	REPLACE sub_total WITH subtotal
	REPLACE freight WITH frght
	REPLACE tax_rate WITH tx_rate
	REPLACE taxes WITH tx
	REPLACE total WITH ttl - frght 
	REPLACE end_total WITH subtotal + tx + frght
	
	
*!*		APPEND BLANK
*!*		GATHER NAME oOrdrstat
*!*		RELEASE oOrdrstat
	

ENDPROC 


PROCEDURE quote_populate_order_de
	PARAMETERS orderno
		
	USE quote_detail IN 21 SHARED 
	SET ORDER TO 4
	
	PATH = ""
	DO ORDRPATH.PRG		
	USE PATH + "ORDER_DE" IN 22 SHARED AGAIN 	
		
	USE "CUSTOM" IN 23 SHARED 
	
	SELECT 21	
	
	SCAN FOR ALLTRIM(quote_no) == ALLTRIM(quoteno) AND NOT DELETED()
		
		SELECT 21
		STORE entry_no TO entryno
		STORE part TO cPart
		STORE quant TO nQuant	
		STORE descrption TO Description
		STORE taxable to tax
		STORE cost TO nCost
		STORE total_cost TO nTotal_cost
		STORE custom TO cCustom	
		STORE acct_num TO cAcctNum
		REPLACE new_order WITH orderno
				
		SELECT 22		
		SCATTER NAME oOrder_de BLANK 
		oOrder_de.co_id = coid
		oOrder_de.order_no = orderno
		oOrder_de.entry_no = entryno
		oOrder_de.part = cPart
		oOrder_de.quant = nQuant
		oOrder_de.descript = Description
		oOrder_de.taxable = tax
		oOrder_de.cost = nCost
		oOrder_de.total_cost = nTotal_cost
		oOrder_de.custom = cCustom
		oOrder_de.acct_num = cAcctNum
		
		
		APPEND BLANK
		GATHER NAME oOrder_de
		
		IF !EMPTY(cCustom)
			SELECT 23
			APPEND BLANK 
			REPLACE CO_ID WITH CONO
		   	REPLACE ORDER_NO WITH ORDERNO
		   	REPLACE DESCRIPT WITH DESCRIPTION
		   	REPLACE PART_NO WITH cPART
		   	REPLACE CUSTOM WITH cCUSTOM
		   	REPLACE COST WITH nCOST
		   	SELECT 22
		ENDIF	
			
		RELEASE oOrder_de
		
				
	ENDSCAN 	
	
	MESSAGEBOX('ORDER # '+ ALLTRIM(ORDERNO), 0, "Quote Successfully Converted to Order")

ENDPROC 


