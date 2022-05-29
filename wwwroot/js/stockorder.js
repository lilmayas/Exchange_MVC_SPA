
// document.addEventListener("DOMContentLoaded", orderStart);

(function () {
    loadOrders();
    const sendButton = document.getElementById("button-send");
    if (!sendButton) throw "button-send not found";
    sendButton.addEventListener("click", sendButtonClick);
})()

async function loadOrders() {
    // Запрос заказов (JSON)
    fetch("/api/order").then(r => r.json()).then(j => {  // j - [{id:, moment:, cnt:},{}]
        // console.log(j);
        // Запрос шаблона отображения (/tpl/)
        fetch("/tpl/odertpl.html").then(r => r.text()).then(t => {
            // console.log(t);

            var container = "";  // формируем содержимое HTML
            // заполнение шаблона циклом по заказам
            for (let order of j) {  // order - {id:, moment:, cnt:}
                /* var template = t;  // исходный шаблон
                // замена вставок {{id}} на order.id
                template = fillTemplate(t, order);
                // console.log(template);
                // добавляем заполненный шаблон в HTML
                container += template; */
                container += fillTemplate(t, order);
            }
            // вставляем заполненные шаблоны в <div id="orders-container">
            document.querySelector("#orders-container")
                .innerHTML = container;
        })
    })
}

/**
 * Заполнение шаблона данными из объекта
 * @param {string} tpl Шаблон
 * @param {any} obj Данные
 */
function fillTemplate(tpl, obj) {
    for (let prop in obj) {  // цикл по свойствам объекта id, moment, cnt...
        var reg = new RegExp(`{{${prop}}}`, 'g'); 
        tpl = tpl.replace(reg, obj[prop]);
    }
    return tpl;
}

function sendButtonClick(e) {
    // определяем, какая радиокнопка выбрана
    const rb = document.querySelector(
        "input[name='sel-method']:checked");
    if (rb == null) {
        alert("Select method");
        return;
    }
    const container = document.querySelector("#order-container");
    if (!container) throw "order-container not found";

    switch(rb.id){
        case "rb1" :
            // console.log("GET");
            fetch("/api/order")
                .then(r => r.text())
                .then(t => container.innerText = t);
            break;
        case "rb2":
            // console.log("POST");
            fetch("/api/order", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify("Привет серверу!")
            })
                .then(r => r.text())
                .then(t => container.innerText = t);
            break;
        case "rb3":
            // console.log("PUT");
            fetch("/api/order/32", {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify("Пут серверу!")
            })
                .then(r => r.text())
                .then(t => container.innerText = t);
            break;
        case "rb4":
            // console.log("DELETE");
            fetch("/api/order/32", {
                method: "DELETE"
            })
                .then(r => r.text())
                .then(t => container.innerText = t);
            break;
        default:
            console.error("Unknown id");
    }
}