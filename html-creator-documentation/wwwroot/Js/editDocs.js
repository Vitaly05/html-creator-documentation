class Block {
    constructor(type, innerBlock) {
        this.type = type
        this.innerBlockType = innerBlock
    }
}

const blocks = [
    new Block('article', 'large-block'),
    new Block('large-block', 'big-block'),
    new Block('big-block', 'small-block'),
    new Block('small-block', 'block')
]

turnOnEditMode()

function turnOnEditMode() {
    blocks.forEach(block => {
        $(`.${block.innerBlockType}`).mouseenter(function(e) {
            e.preventDefault()
            e.stopPropagation()
            $(this).parents().removeClass('editable-block')
            $(this).addClass('editable-block')
        })
        $(`.${block.innerBlockType}`).mouseleave(function() {
            $(this).removeClass('editable-block')
        })
        $(`.${block.type}`).sortable({
            placeholder: 'block-placeholder',
            cursor: 'move',
            handle: `.move-button[data-target="${block.innerBlockType}"]`,
            items: `.${block.innerBlockType}`
        })
    })
    $('.empty').mouseenter(function(e) {
        e.stopPropagation()
        $(this).parent().addClass('editable-block')
    })

    $('.delete-button').click(function() {
        $(this).parent().parent().remove()
    })

    $('.new-item button').mouseenter(function(e) {
        e.stopPropagation()
        $(this).parent().parent().addClass('editable-block')
    })
    $(`.new-item button`).mouseleave(function() {
        $(this).parent().parent().removeClass('editable-block')
    })
    $('.new-item button').click(function() {
        let currentBlock = $(this).parent().parent()
        let currentBlockType = currentBlock.data('block-type')
        let newBlockType = ''
    
        blocks.forEach(block => {
            if (block.type == currentBlockType) {
                newBlockType = block.innerBlockType
            }
        })
    
        newBlockContainer = currentBlock
        showDialog(newBlockType)
    })

    $('.value').attr('contenteditable', 'true')
    $('h2.title').attr('contenteditable', 'true')
}


var newBlockContainer

const newBlockDialog = $('#new-block-dialog').dialog({
    autoOpen: false,
    height: 500,
    width: 700,
    modal: true,
    buttons: [
        {
            text: 'Добавить',
            click: function() {
                $.ajax({
                    url: 'docs',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify(GetNewElementJson()),
                    success: function(data) {
                        newBlockContainer.find('.new-item:last').before(data)
                        turnOnEditMode()
                        newBlockDialog.dialog('close')
                    }
                })
            }
        },
        {
            text: 'Отмена',
            click: function() {
                $(this).dialog('close')
            }
        }
    ]
})


const listItemTemplate = $('#new-block-dialog .list-item:first').clone()

$('#new-block-dialog #new-list-item-button').click(function(e) {
    e.preventDefault()
    $(e.target).before(listItemTemplate.clone())
    addDialogRemoveButtonsEvents()
})

addDialogRemoveButtonsEvents()

function addDialogRemoveButtonsEvents() {
    $('.remove-list-item-button').click(function() {
        $(this).parent().remove()
    })
}

function GetNewElementJson() {
    let json = {}

    if ($('#block-type-select').selectmenu('option', 'disabled')) {
        json.type = newBlockDialog.data('block-type')
    } else {
        json.type = $('#block-type-select').val()
    }

    switch (json.type) {
        case 'list':
        case 'code':
            json.listValues = GetListValues($('#list-items'))
            break
        case 'description':
        case 'tip':
            json.value = $('#single-item textarea').val()
            break
        default:
            json.title = $('#single-item textarea').val()
    }

    return json
}

function GetListValues(listItemsBlock) {
    let values = []
    listItemsBlock.find('textarea').each(function() {
        values.push($(this).val())
    })
    return values
}

function showDialog(type) {
    newBlockDialog.find('form')[0].reset()
    newBlockDialog.data('block-type', type)
    newBlockDialog.find('#list-items').hide()
    newBlockDialog.find('#single-item').show()

    $('#block-type-select').selectmenu({ change: changeBlockType })
    $('#block-type-select').hide()

    setFieldsParams(type)
    newBlockDialog.dialog('open')
}

function setFieldsParams(type) {
    switch (type) {
        case 'large-block':
            newBlockDialog.data('block-type', 'large-block')
            setupDialogAs(true)
            break
        case 'big-block':
            newBlockDialog.data('block-type', 'big-block')
            setupDialogAs(true)
            break
        case 'small-block':
            newBlockDialog.data('block-type', 'small-block')
            setupDialogAs(true)
            break
        case 'block':
            newBlockDialog.data('block-type', 'block')
            setupDialogAs(false)
            break
    }
}
function setupDialogAs(isTitleBlock) {
    if (isTitleBlock) {
        $('#block-type-select').selectmenu('disable')
        newBlockDialog.find('textarea').attr('name', 'Title')
        newBlockDialog.find('label:first').text('Заголовок')
        newBlockDialog.find('#list-items').hide()
    } else {
        $('#block-type-select').selectmenu('enable')
        newBlockDialog.find('textarea').attr('name', 'Value')
        newBlockDialog.find('label:first').text('Текст')
        newBlockDialog.find('#list-items').hide()
    }
}

function changeBlockType(e, data) {
    switch (data.item.value) {
        case 'code':
        case 'list':
            newBlockDialog.find('#list-items').show()
            newBlockDialog.find('#single-item').hide()
            break
        default:
            newBlockDialog.find('#list-items').hide()
            newBlockDialog.find('#single-item').show()
    }
}



const articleName = $('.article-title').text()

let updateArticleRequest = getHttpRequest()
updateArticleRequest.open('POST', `/documentation/update/${articleName}`, true)
updateArticleRequest.setRequestHeader('Content-Type', 'application/json')
updateArticleRequest.setRequestHeader('Authorization', `Bearer ${sessionStorage.getItem('accessToken')}`)

updateArticleRequest.onreadystatechange = function() {
    if (updateArticleRequest.readyState == 4) {
        if (updateArticleRequest.status == 200) {
            window.sessionStorage.removeItem('accessToken')
            window.location.href = `/docs?topic=${articleName}`
        }
        else if (updateArticleRequest.status == 500) {
            console.error('Ошибка сервера: Не удалось обновить статью')
        }
        else if (updateArticleRequest.status == 401) {
            alert('Время доступа истекло')
            window.location.href = 'docs'
        }
    }
}

$('#save-button').click(function() {
    updateArticleRequest.send(GetArticleJson())
})

function getHttpRequest() {
    let httpRequest
    if (window.XMLHttpRequest) {
        httpRequest = new XMLHttpRequest()
    } else if (window.ActiveXObject) {
        httpRequest = new ActiveXObject("Microsoft.XMLHTTP");
    }
    return httpRequest
}

function GetArticleJson() {
    let json = []
    $('.article > .block-item').each(function() {
        json.push(GetElementJson($(this)))
    })
    return JSON.stringify(json)
}
function GetElementJson(element) {
    let json = {}

    const type = element.data('block-type')
    json.type = type

    const title = element.find('.block-title:first').text()
    json.title = title

    const innerElements = element.find(`> .block-item`)
    if (innerElements.length > 0) {
        json.elements = []
        innerElements.each(function() {
            json.elements.push(GetElementJson($(this)))
        })
    } else {
        switch (type) {
            case 'description':
            case 'tip':
                const value = element.find('.value:first').text()
                if (value != '') {
                    json.value = value
                }
                break
            case 'code':
            case 'list':
                json.listValues = []
                element.find('.value').each(function() {
                    json.listValues.push($(this).text())
                })
                break
        }
    }
    
    return json
}