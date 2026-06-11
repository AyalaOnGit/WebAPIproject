const fetchWithRetry = async (url, options, retries = 3, delayMs = 1000) => {
    for (let i = 0; i < retries; i++) {
        try {
            const response = await fetch(url, options);
            if (response.status !== 503 && response.status !== 429) return response;
        } catch (err) {
            if (i === retries - 1) throw err;
        }
        await new Promise(r => setTimeout(r, delayMs * (i + 1)));
    }
}

const welcomeText = document.querySelector(".greeting")
welcomeText.innerHTML = `שלום ${JSON.parse(sessionStorage.getItem('currentUser')).userFirstName} <br> !התחברת בהצלחה`

const updateBox = document.querySelector(".updateBox")
const updateUser = document.querySelector(".updateUser")
updateUser.style.display = 'none'

updateBox.addEventListener("click", () => {
    updateUser.style.display = 'block'
})

function validateEmail(email) {
    return email.includes("@") && email.includes(".");
}

const userEmail = document.querySelector("#userName")
const firstName = document.querySelector("#firstName")
const lastName = document.querySelector("#lastName")
const password = document.querySelector("#password")

const currentUser = sessionStorage.getItem("currentUser")
const theCurrentUser = JSON.parse(currentUser)

const update = async () => {
    try {
        if (!validateEmail(userEmail.value)) {
            alert("כתובת האימייל אינה תקינה");
            return;
        }

        if (password.value.length < 4 || password.value.length > 20) {
            alert("הסיסמה חייבת להיות בין 4 ל-20 תווים");
            return;
        }

        const updateUser = {
            UserId: theCurrentUser.userId,
            UserEmail: userEmail.value,
            UserFirstName: firstName.value,
            UserLastName: lastName.value,
            UserPassword: password.value
        }
        const url = new URL(`https://localhost:44324/api/Users/${theCurrentUser.userId}`)
        const response = await fetchWithRetry(url, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            credentials: 'include',
            body: JSON.stringify(updateUser)
        });
        if (response.ok) {
            alert(" עודכן בהצלחה")
            const updatedUser = {
                userId: theCurrentUser.userId,
                userEmail: userEmail.value,
                userFirstName: firstName.value,
                userLastName: lastName.value,
                isAdmin: theCurrentUser.isAdmin
            }
            sessionStorage.setItem("currentUser", JSON.stringify(updatedUser))
            welcomeText.innerHTML = `שלום ${updatedUser.userFirstName} <br> !עודכן בהצלחה`
        }

        else {
            alert("בעדכון יש בעיה")
        }
    } catch (err) {
        alert(err)
    }
    
}
