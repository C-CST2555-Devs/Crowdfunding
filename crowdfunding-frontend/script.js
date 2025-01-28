document.addEventListener("DOMContentLoaded", function () {
    fetch("http://localhost:5000/api/campaigns")
        .then(response => response.json())
        .then(data => {
            const list = document.getElementById("campaigns-list");
            list.innerHTML = "";
            data.forEach(campaign => {
                const item = document.createElement("li");
                item.textContent = `${campaign.title} - Goal: $${campaign.goalAmount}`;
                list.appendChild(item);
            });
        })
        .catch(error => console.error("Error fetching campaigns:", error));
}); 