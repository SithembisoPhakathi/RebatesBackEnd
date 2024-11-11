
    window.onload = function () {
            var today = new Date().toISOString().split('T')[0];
    document.getElementsByName("PaymentDate")[0].setAttribute('max', today);
        document.getElementsByName("StartDate")[0].setAttribute('max', today);
        document.getElementsByName("StartDate1")[0].setAttribute('max', today);
        document.getElementsByName("StartDate2")[0].setAttribute('max', today);
        document.getElementsByName("AgriClaim")[0].setAttribute('max', today);
        document.getElementsByName("AgriStart")[0].setAttribute('max', today);
        }


