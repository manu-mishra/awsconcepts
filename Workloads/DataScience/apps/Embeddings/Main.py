import os
import json
from transformers import RobertaModel, RobertaTokenizerFast
import torch

def model_fn(model_dir):
    tokenizer = RobertaTokenizerFast(
        vocab_file=os.path.join(model_dir, "awsblogs-vocab.json"), 
        merges_file=os.path.join(model_dir, "awsblogs-merges.txt"),
        bos_token="<s>",
        eos_token="</s>",
        sep_token="</s>",
        cls_token="<s>",
        unk_token="<unk>",
        pad_token="<pad>",
        mask_token="<mask>"
    )

    model = RobertaModel.from_pretrained('roberta-base')
    
    return {'tokenizer': tokenizer, 'model': model}

def input_fn(request_body, request_content_type):
    if request_content_type == "application/json":
        request_body = json.loads(request_body)
        if isinstance(request_body, str):
            return request_body
        elif isinstance(request_body, list) and len(request_body) > 0 and isinstance(request_body[0], str):
            return request_body
        else:
            raise ValueError("Received input in an unexpected format.")
    else:
        raise ValueError("Received request with content type: {}".format(request_content_type))

def predict_fn(input_object, model):
    tokenizer = model['tokenizer']
    model = model['model']
    
    inputs = tokenizer(input_object, return_tensors='pt')
    outputs = model(**inputs)
    embeddings = outputs.last_hidden_state.mean(dim=1)
    vectors = embeddings.detach().numpy().tolist()
    
    return vectors

def output_fn(prediction, response_content_type):
    if response_content_type == "application/json":
        return json.dumps(prediction), response_content_type
    else:
        raise ValueError("Received request with response type: {}".format(response_content_type))
