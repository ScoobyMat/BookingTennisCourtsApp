document.addEventListener("DOMContentLoaded", function () {
    var startTimeSelect = document.getElementById("Reservation_StartTime");
    var endTimeSelect = document.getElementById("Reservation_EndTime");

    startTimeSelect.addEventListener("change", function () {
        updateEndTimeOptions();
    });

    function updateEndTimeOptions() {
        var selectedStartTime = parseInt(startTimeSelect.value);

        // Usuń wszystkie opcje zakończenia, które są wcześniejsze niż godzina rozpoczęcia
        for (var i = 10; i <= 18; i++) {
            if (i <= selectedStartTime) {
                endTimeSelect.querySelector("option[value='" + i + ":00']").disabled = true;
            } else {
                endTimeSelect.querySelector("option[value='" + i + ":00']").disabled = false;
            }
        }
    }

    // Inicjalizacja opcji zakończenia na podstawie początkowej godziny
    updateEndTimeOptions();
});
