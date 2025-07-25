OPEN DATABASE w:\foxapps\Shortdata
MODIFY DATABASE
REMOVE TABLE company
REMOVE TABLE contact
REMOVE TABLE quote_header
REMOVE TABLE quote_detail
REMOVE TABLE quote_comment
REMOVE TABLE ordrstat
REMOVE TABLE order_de
REMOVE TABLE ordetail

*Lookup Tables
REMOVE TABLE partslst 
REMOVE TABLE partsize
REMOVE TABLE users
REMOVE TABLE ship_via
*!*	REMOVE TABLE disc_code
*!*	REMOVE TABLE terms_code
*!*	REMOVE TABLE subacct_code
*!*	REMOVE TABLE tax_code
*!*	REMOVE TABLE acct_code
*!*	REMOVE TABLE qa_code

*Number Lookups
REMOVE TABLE quote_no
REMOVE TABLE order_no
REMOVE TABLE p_ord_no
REMOVE TABLE conumber
REMOVE TABLE custom_i

REMOVE TABLE pay_dtl
REMOVE TABLE overdue
REMOVE TABLE shipping
REMOVE TABLE serialno
REMOVE TABLE model_no


REMOVE TABLE invoice
REMOVE TABLE amount
REMOVE TABLE misc_chk
REMOVE TABLE precheck


*Shrtridg Directory
COPY FILE w:\foxapps2\foxapp\shrtridg\company.dbf TO w:\foxapps\shrtridg\company.dbf
COPY FILE w:\foxapps2\foxapp\shrtridg\contact.dbf TO w:\foxapps\shrtridg\contact.dbf
COPY FILE w:\foxapps2\foxapp\shrtridg\quote_header.dbf TO w:\foxapps\shrtridg\quote_header.dbf
COPY FILE w:\foxapps2\foxapp\shrtridg\quote_detail.dbf TO w:\foxapps\shrtridg\quote_detail.dbf
COPY FILE w:\foxapps2\foxapp\shrtridg\quote_comment.dbf TO w:\foxapps\shrtridg\quote_comment.dbf
COPY FILE w:\foxapps2\foxapp\shrtridg\ordrstat.dbf TO w:\foxapps\shrtridg\ordrstat.dbf
COPY FILE w:\foxapps2\foxapp\shrtridg\ordetail.dbf TO w:\foxapps\shrtridg\ordetail.dbf
COPY FILE w:\foxapps2\foxapp\shrtridg\quote_no.dbf TO w:\foxapps\shrtridg\quote_no.dbf
COPY FILE w:\foxapps2\foxapp\shrtridg\conumber.dbf TO w:\foxapps\shrtridg\conumber.dbf
COPY FILE w:\foxapps2\foxapp\shrtridg\custom_i.dbf TO w:\foxapps\shrtridg\custom_i.dbf
COPY FILE w:\foxapps2\foxapp\shrtridg\overdue.dbf TO w:\foxapps\shrtridg\overdue.dbf
COPY FILE w:\foxapps2\foxapp\shrtridg\shipping.dbf TO w:\foxapps\shrtridg\shipping.dbf
COPY FILE w:\foxapps2\foxapp\shrtridg\serialno.dbf TO w:\foxapps\shrtridg\serialno.dbf
COPY FILE w:\foxapps2\foxapp\shrtridg\model_no.dbf TO w:\foxapps\shrtridg\model_no.dbf
COPY FILE w:\foxapps2\foxapp\shrtridg\invoice.dbf TO w:\foxapps\shrtridg\invoice.dbf
COPY FILE w:\foxapps2\foxapp\shrtridg\misc_chk.dbf TO w:\foxapps\shrtridg\misc_check.dbf
COPY FILE w:\foxapps2\foxapp\shrtridg\partslst.dbf TO w:\foxapps\shrtridg\partslst.dbf
COPY FILE w:\foxapps2\foxapp\shrtridg\partsize.dbf TO w:\foxapps\shrtridg\partsize.dbf
COPY FILE w:\foxapps2\foxapp\shrtridg\users.dbf TO w:\foxapps\shrtridg\users.dbf
COPY FILE w:\foxapps2\foxapp\shrtridg\ship_via.dbf TO w:\foxapps\shrtridg\ship_via.dbf

*Amount Directory
COPY FILE w:\Amount2\amount.dbf TO w:\foxapps\amount\amount.dbf
COPY FILE w:\Amount2\pay_dtl.dbf TO w:\foxapps\amount\pay_dtl.dbf

*Callibration Directory
COPY FILE w:\foxapps2\foxapp\calibration\cal_info.dbf TO w:\foxapps\calibration\cal_info.dbf

*Repairs Directory
COPY FILE w:\foxapps2\foxapp\repairs\precheck.dbf TO w:\foxapps\foxapp\repairs\precheck.dbf
COPY FILE w:\foxapps2\foxapp\repairs\labels.dbf TO w:\foxapps\foxapp\repairs\labels.dbf
COPY FILE w:\foxapps2\foxapp\repairs\order_no.dbf TO w:\foxapps\foxapp\repairs\order_no.dbf
COPY FILE w:\foxapps2\foxapp\repairs\pc_misc.dbf TO w:\foxapps\foxapp\repairs\pc_misc.dbf

*Ordrntry Directory
COPY FILE w:\foxapps2\foxapp\ordrntry\ordrstat.dbf TO w:\foxapps\ordrntry\ordrstat.dbf
COPY FILE w:\foxapps2\foxapp\ordrntry\order_de.dbf TO w:\foxapps\ordrntry\order_de.dbf
COPY FILE w:\foxapps2\foxapp\ordrntry\order_no.dbf TO w:\foxapps\ordrntry\order_no.dbf
COPY FILE w:\foxapps2\foxapp\ordrntry\p_ord_no.dbf TO w:\foxapps\ordrntry\p_ord_no.dbf


ADD TABLE w:\foxapps\shrtridg\company
ADD TABLE w:\foxapps\shrtridg\contact
ADD TABLE w:\foxapps\shrtridg\quote_header
ADD TABLE w:\foxapps\shrtridg\quote_detail
ADD TABLE w:\foxapps\shrtridg\quote_comment
ADD TABLE w:\foxapps\shrtridg\ordrstat
ADD TABLE w:\foxapps\shrtridg\ordetail
ADD TABLE w:\foxapps\shrtridg\quote_no
ADD TABLE w:\foxapps\shrtridg\conumber
ADD TABLE w:\foxapps\shrtridg\custom_i
ADD TABLE w:\foxapps\shrtridg\overdue
ADD TABLE w:\foxapps\shrtridg\shipping
ADD TABLE w:\foxapps\shrtridg\serialno
ADD TABLE w:\foxapps\shrtridg\model_no
ADD TABLE w:\foxapps\shrtridg\invoice
ADD TABLE w:\foxapps\shrtridg\misc_check
ADD TABLE w:\foxapps\shrtridg\partslst
ADD TABLE w:\foxapps\shrtridg\partsize
ADD TABLE w:\foxapps\shrtridg\users
ADD TABLE w:\foxapps\shrtridg\ship_via
ADD TABLE w:\foxapps\amount\amount
ADD TABLE w:\foxapps\amount\pay_dtl
ADD TABLE w:\foxapps\calibration\cal_info
ADD TABLE w:\foxapps\repairs\precheck
ADD TABLE w:\foxapps\repairs\labels
ADD TABLE w:\foxapps\repairs\order_no
ADD TABLE w:\foxapps\repairs\pc_misc
ADD TABLE w:\foxapps\ordrntry\ordrstat
ADD TABLE w:\foxapps\ordrntry\order_de
ADD TABLE w:\foxapps\ordrntry\order_no
ADD TABLE w:\foxapps\ordrntry\p_ord_no