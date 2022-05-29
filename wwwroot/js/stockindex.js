document.addEventListener("DOMContentLoaded", function () {
    loadItemsCount();
    loadStockItems();
});

async function loadItemsCount() {
    const itemCount = document.getElementById("itemCount");
    if (!itemCount) throw "itemCount container not found";

    fetch("/Stock/Items?count")
        .then(r => r.json())
        .then(j => {
            // console.log(j);
            if (typeof j.count === 'undefined')
                throw "Sever answer contains no COUNT field";
            itemCount.innerText = j.count;
        })
}

async function loadStockItems() {
    const container = document.getElementById("stockContainer");
    if (!container) throw "stockContainer container not found";

    fetch("/Stock/Items")
        .then(r => r.json())
        .then(j => {
            // container.innerHTML = j;
            /*for (let item of j) {
                
                container.innerHTML +=
                    "<p>"
                + `<img src='/img/${item.logoFilename}' />`
                    + item.title
                    + " "
                    + item.sellRate
                    + "</p>";
            }*/
            /*
            // В виде таблицы. 
            var tbl = "<table border=2>";
            for (let item of j) {
                tbl += `
                <tr>
                    <td><img style='width:50px' src='/img/${item.logoFilename}' /></td>
                    <td>${item.title}</td>
                    <td>${item.sellRate}</td>
                    <td>${item.buyRate}</td>
                </tr>
                `;
            }
            tbl += "</table>";
            container.innerHTML = tbl;
            */
            /*
            // сделать шаблон в который будут подставляться данные
            var template = "<p class='stock-item'><b>{{title}}</b></p>";
            var html = "";
            for (let item of j) {
                html += template.replace("{{title}}", item.title);
            }
            container.innerHTML = html;
            */

            // получать шаблон с сервера
            fetch("/tpl/stockitem.html")
                .then(r => r.text())
                .then(template => {
                    // console.log(template);
                    var html = "";
                    // const placeholders = ["title","logoFilename","description"];
                    for (let item of j) {
                        if (item.isVisible == false) continue;
                        let t = template;
                        // placeholders.forEach(p => t = t.replace("{{" + p + "}}", item[p]));
                        for (let prop in item) {
                            let search = "{{" + prop + "}}";
                            while (t.includes(search))
                                t = t.replace(search, item[prop]);
                        }
                        html += t;
                        /*html += template
                            .replace("{{title}}", item.title)
                            .replace("{{logoFilename}}", item.logoFilename);*/
                    }
                    container.innerHTML = html;
                });
        });
}

function stockItemClick(itemId) {
    // console.log(itemId);
    // Отображение детальной информации о "позиции"
    
    // 1. Получаем данные от сервера
    fetch("/Stock/Items/" + itemId)
        .then(r => r.json())
        .then( showItemDtl )
        /*
        .then(t => {
            itemContainer.innerHTML = t;
            
        })*/
}

async function showItemDtl(j) {
    // 2. Функция получает j: JSON ответ сервера - детали по stockItem
    // запрашиваем шаблон отображения
    fetch("/tpl/itemdtl.html")
        .then(r => r.text())
        .then(t => {
            // t - HTML текст шаблона
            // j - данные для подстановки в шаблон
            // проходим циклом по всем полям объекта j и заменяем
            // их {{имена}} значениями в шаблоне
            for (let key in j) {
                // key - "пробегает" поля: id, title, ...
                // j[key] - значения соотв. полей
                let search = `{{${key}}}`;
                while (t.includes(search))
                    t = t.replace(search, j[key]);
            }
            // после цикла в t - HTML с подставленными данными
            // помещаем его в контейнер itemContainer
            const itemContainer = document.getElementById("itemContainer");
            if (!itemContainer) throw "itemContainer not found";
            itemContainer.innerHTML = t;
        })
}

function stockItemOrder(id) {
    // извлечь значение из input рядом с кнопкой
    let cnt = 
        event               // данные о событии
            .target         // источник - кнопка
            .parentNode     // родитель
            .querySelector("input[type=number]")
            .value;
    // Отсылаем на заказ: id-item, cnt-Count
    // console.log(cnt);
    fetch("/api/order", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({ idItem: id, cnt: cnt})
    })
        .then(r => r.text())
        .then(console.log);
}
