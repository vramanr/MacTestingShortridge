OPEN DATABASE W:\foxapps\shortdata

SELECT DISTINCT invoice.co_id, invoice.co_name, invoice.order_no, inv_date, part FROM invoice, ordetail WHERE invoice.co_id = ordetail.co_id;
AND invoice.order_no = ordetail.order_no AND inv_date between CTOD('05/14/03') and CTOD('08/13/03');
AND part = "MT-440K"

COPY TO  c:\part_query.xls DELIMITED WITH TAB

MESSAGEBOX("File saved to c:\part_query.xls.")