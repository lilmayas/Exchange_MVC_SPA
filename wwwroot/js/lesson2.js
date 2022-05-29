document.addEventListener("DOMContentLoaded", function () {
    // console.log("Script loaded")
    const postDiv = document.getElementById("post-api");
    if (!postDiv) throw "post-api div not located";
    postDiv.querySelector("button").addEventListener("click", postClick);
});

function postClick(e) {
    const out = e.target.parentNode.querySelector("span");
    fetch("/Home/RndValue?base=2000", {
        method: "POST",
        headers: {
            // "Content-Type": "application/x-www-form-urlencoded"
            "Content-Type": "application/json"
        },
        body: '{"Base":1000, "Quantity":5}'
    }).then(r => r.text())
      .then(t => { out.innerText = t; });
}