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
    e.preventDefault()
    e.stopPropagation()
    $(this).parent().addClass('editable-block')
})

$('.delete-button').click(function() {
    $(this).parent().parent().remove()
})


const newBlockDialog = $('#new-block-dialog').dialog({
    autoOpen: false,
    height: 300,
    width: 400,
    modal: true,
    buttons: [
        {
            text: 'Добавить',
            click: function() {
                
            }
        },
        {
            text: 'Отмена',
            click: function() {
                $(this).find('form')[0].reset()
                $(this).dialog('close')
            }
        }
    ]
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

    showDialog(newBlockType)
})

function showDialog(type) {
    newBlockDialog.find('form')[0].reset()
    setFieldsParams(type)
    newBlockDialog.data('block-type', type)
    newBlockDialog.dialog('open')
}

function setFieldsParams(type) {
    switch (type) {
        case 'large-block':
        case 'big-block':
        case 'small-block':
            newBlockDialog.find('label').text('Title')
            break
        case 'block':
            newBlockDialog.find('label').text('Content')
            break
    }
}


$('.value').attr('contenteditable', 'true')
$('h2.title').attr('contenteditable', 'true')

const articleName = $('.article-title').text()

let updateArticleRequest = getHttpRequest()
updateArticleRequest.open('POST', `/documentation/update/${articleName}`, true)
updateArticleRequest.setRequestHeader('Content-Type', 'application/json')

updateArticleRequest.onreadystatechange = function() {
    if (updateArticleRequest.readyState == 4) {
        if (updateArticleRequest.status == 200) {
            window.location.href = `/docs?topic=${articleName}`
        }
        else if (updateArticleRequest.status == 500) {
            console.error('Ошибка сервера: Не удалось обновить статью')
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