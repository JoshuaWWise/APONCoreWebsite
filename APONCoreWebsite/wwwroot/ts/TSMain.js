function CheckIfAdmin(currentLevel) {
    if (currentLevel > 2) {
        window.location.href = "/Index";
    }
}
function CheckIfEditor(currentLevel) {
    if (currentLevel > 3) {
        window.location.href = "/Index";
    }
}
function CheckIfHighAdmin(currentLevel) {
    if (currentLevel > 1) {
        window.location.href = "/Index";
    }
}
