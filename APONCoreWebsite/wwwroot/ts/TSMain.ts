
function  CheckIfAdmin(currentLevel: number) {


    if (currentLevel > 2) {
        window.location.href ="/Index";
    }
}

function CheckIfEditor(currentLevel: number) {
    if (currentLevel > 3) {
        window.location.href = "/Index";
    }
}

function CheckIfHighAdmin(currentLevel: number) {
    if (currentLevel > 1) {
        window.location.href = "/Index";
    }
}