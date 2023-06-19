const accessToken = window.sessionStorage.getItem('accessToken')
setAccessToken(accessToken)

const loginInput = $('#login-field')
const passwordInput = $('#password-field')

const authDialog = $('#auth-dialog').dialog({
    autoOpen: false,
    height: 500,
    width: 700,
    modal: true,
    buttons: [
        {
            text: 'Войти',
            click: async function () {
                if (authForm.valid()) {
                    await fetch(`documentation/getToken?login=${loginInput.val()}&password=${passwordInput.val()}`)
                        .then(async response => {
                            if (response.ok) {
                                const accessToken = await response.json()
                                window.sessionStorage.setItem('accessToken', accessToken)
                                setAccessToken(accessToken)
                            } else {
                                $('#invalid-auth-data').show()
                            }
                        })
                }
            }
        },
        {
            text: 'Отмена',
            click: function () {
                $(this).dialog('close')
            }
        }
    ]
})

const authForm = authDialog.children('form')
authForm.validate({
    rules: {
        login: {
            required: true,
            minlength: 4,
            maxlength: 20
        },
        password: {
            required: true,
            minlength: 4,
            maxlength: 20
        }
    },
    messages: {
        login: {
            required: 'Введите логин',
            minlength: 'Минимальная длинна логина 4 символа',
            maxlength: 'Максимальная длинна логина 20 символов'
        },
        password: {
            required: 'Введите пароль',
            minlength: 'Минимальная длинна пароля 4 символа',
            maxlength: 'Максимальная длинна пароля 20 символов'
        },
    }
})

$('#edit-article-button').click(function() {
    authDialog.dialog('open')
})

function setAccessToken(accessToken) {
    if (accessToken != null && !window.location.href.includes(accessToken)) {
        if (window.location.href.includes('#')) {
            const anchor = window.location.href.slice(window.location.href.indexOf('#'), window.location.href.length)
            window.location.href = window.location.href.replace(anchor, '')
            console.log(anchor)
        }
        let symbol = window.location.href.includes('?') ? '&' : '?'
        window.location.href += `${symbol}accessToken=${accessToken}`
    }
}