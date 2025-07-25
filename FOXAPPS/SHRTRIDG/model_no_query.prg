OPEN DATABASE W:\foxapps\shortdata

SELECT co_id, co_name, order_no, model_no, serial_no, cal_date  from model_no WHERE (RIGHT(ALLTRIM(model_no), 3) = "150";
OR RIGHT(ALLTRIM(model_no), 3) = "300" OR RIGHT(ALLTRIM(model_no), 4) = "150M" OR;
RIGHT(ALLTRIM(model_no), 4) = "300M" OR RIGHT(ALLTRIM(model_no), 5) = "150-W" OR RIGHT(ALLTRIM(model_no), 5) = "300-W") AND;
cal_date between CTOD('05/14/03') and CTOD('08/13/03') ORDER BY serial_no

COPY TO c:\model_no_query.xls DELIMITED WITH TAB

MESSAGEBOX("File saved as c:\model_no_query.xls")
