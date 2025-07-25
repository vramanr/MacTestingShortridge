OPEN DATABASE W:\foxapps\shortdata

SELECT a.order_no, a.custom, b.order_no, a.custom ;
FROM ordetail a, ordetail b ;
WHERE a.custom = b.custom and a.custom <> " " AND a.order_no <> b.order_no;
ORDER BY a.custom



SELECT a.order_no, a.custom, b.order_no, a.custom ;
FROM custom a, custom b ;
WHERE a.custom = b.custom and a.custom <> " " AND a.order_no <> b.order_no;
ORDER BY a.custom




SELECT a.order_no, a.custom, b.order_no, a.custom ;
FROM order_de a, order_de b ;
WHERE a.custom = b.custom and a.custom <> " " AND a.order_no <> b.order_no;
ORDER BY a.custom



SELECT a.new_order, a.custom, b.new_order, a.custom ;
FROM quote_detail a, quote_detail b ;
WHERE a.custom = b.custom and a.custom <> " " AND a.new_order <> b.new_order;
ORDER BY a.custom
