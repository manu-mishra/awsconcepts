import os
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

def predict_fn(input_object, model):
    tokenizer = model['tokenizer']
    model = model['model']
    
    inputs = tokenizer(input_object, return_tensors='pt')
    outputs = model(**inputs)
    embeddings = outputs.last_hidden_state.mean(dim=1)
    vectors = embeddings.detach().numpy().tolist()
    
    return vectors
### test sample
if __name__ == "__main__":
    model_dir = os.path.dirname(os.path.realpath(__file__))
    model = model_fn(model_dir)
    result = predict_fn("Some text for testing", model)
    print(result)