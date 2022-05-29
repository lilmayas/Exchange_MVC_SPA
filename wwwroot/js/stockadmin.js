document.addEventListener("DOMContentLoaded", function () {
    loadItemsTable();
    loadLogs();
});

async function loadLogs() {
    fetch("/Home/Logs").then(r => r.json()).then(j => {
        // window.logs.innerText = t
        const tpl = "<p>{{id}} <b>{{url}}</b><p>";
        var logHtml = "";
        for (let log of j) {  // j - {id,url,moment}
            var t = tpl;
            // for( let prop in log ) prop -> id / url / moment
            t = t.replace("{{id}}",  log["id"] );
            t = t.replace("{{url}}", log["url"]);

            logHtml += t;                          
        }
        window.logs.innerHTML = logHtml;
    })
}

async function loadItemsTable() {
    // 1. Данные
    // 2. Шаблон
    // 3. Данные->Шаблон->Контейнер
    fetch("/Stock/Items")
        .then(r => r.json())
        .then( showItemsTable )
}

async function showItemsTable(data) {
    const itemsTable = document.getElementById("itemsTable");
    if (!itemsTable) throw "itemsTable not found";
    // itemsTable.innerHTML = data;
// Задание: если item "удален" отображать кнопку "+", если нет - "Х"
    fetch("/tpl/stockadminitem.html")
        .then(r => r.text())
        .then(t => {
            // console.log(t);
            var tab = "<table>";
            // цикл по data (stock items)
            for (let item of data) {
                // console.log(item);
                let tr = t;  // пустой шаблон
                for (let key in item) {  // заполняем шаблон
                    let search = `{{${key}}}`;
                    while (tr.includes(search))
                        tr = tr.replace(search, item[key]);
                }
                tab += tr;  // добавляем к таблице
            }
            tab += "</table>";
            itemsTable.innerHTML = tab;
        });
}

function switchItem(id, visible) {
    // console.log(id);
    if (visible) {
        fetch("/Stock/Admin/" + id, {
            method: "delete"
        })
        .then(r => r.text())
        .then(loadItemsTable);
    } else {
        fetch("/Stock/Update/" + id, {
            method: "post",
            body: JSON.stringify({ isVisible: true }),
            headers: {
                "Content-Type": "application/json"
            }
        })
        .then(r => r.text())
        .then(loadItemsTable);
    }
}

function editAmount(id) {
    // console.log(event.target);
    event.target.setAttribute("contenteditable", "true");
    event.target.focus();
    event.target.onblur = amountBlur;
    event.target.onkeydown = amountKeyDown;
    event.target.savedValue = event.target.innerText;
    event.target.itemId = id;
}

function amountBlur(e) {
    e.target.removeAttribute("contenteditable");
}

function amountKeyDown(e) {
    // console.log(e.keyCode);
    if (e.keyCode == 27) {  // ESC
        e.target.removeAttribute("contenteditable");
        e.target.innerText = e.target.savedValue;
    }
    if (e.keyCode == 13) {  // Enter
        e.target.removeAttribute("contenteditable");
        // alert(e.target.innerText);
        saveAmount(e.target.itemId, e.target.innerText);
    }
    if (!(e.keyCode >= 48 && e.keyCode <= 57  ||     // keyDigits
          e.keyCode >= 96 && e.keyCode <= 105 ||     // numDigits
          [8, 37, 39, 46].indexOf(e.keyCode) >= 0))  // BackSpace, <-, ->, Del
    {
        e.preventDefault();
        return false;
    }
}

async function saveAmount(id, val) {
    console.log(id, val);
    fetch("/Stock/Update/" + id, {
        method: "post",
        body: JSON.stringify({ amount: val }),
        headers: {
            "Content-Type": "application/json"
        }
    })
    .then(r => r.text())
    .then(console.log);
}

function editLogo(id) {
    const f = document.createElement("input");
    f.type = "file";
    f.itemId = id;
    f.onchange = saveLogo;
    document.body.appendChild(f);
    f.click();
}
function saveLogo(e) {
    const url = "/Stock/UpdateLogo/" + e.target.itemId;
    const formData = new FormData();
    formData.append("logoFile", e.target.files[0]);
    fetch(url, {
        method: "post",
        body: formData
    }).then(r => r.json()).then((j) => {
        console.log(j);
        if (typeof j.status != 'undefined'
            && j.status == 'Error' ) {   // ошибка от сервера
            alert(j.description);
        } else {  // обновление успешно
            loadItemsTable();
        }
        document.body.removeChild(e.target);
    });
}

