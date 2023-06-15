import os
import json
from Main import model_fn, input_fn, predict_fn, output_fn

def test_model():
    model_dir = os.path.dirname(os.path.realpath(__file__))
    model = model_fn(model_dir)
    input_data = json.dumps("This is a test sentence.")
    request_content_type = "application/json"
    response_content_type = "application/json"

    processed_input_data = input_fn(input_data, request_content_type)
    prediction = predict_fn(processed_input_data, model)
    response_body, _ = output_fn(prediction, response_content_type)

    print(response_body)

if __name__ == '__main__':
    test_model()
