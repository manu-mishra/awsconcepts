# Define the base image
# more here https://github.com/aws/deep-learning-containers/blob/master/available_images.md
FROM 763104351884.dkr.ecr.us-east-1.amazonaws.com/pytorch-inference:2.0.1-cpu-py310-ubuntu20.04-sagemaker

# Copy the requirements file into the container
COPY requirements.txt /tmp/
# Install the python dependencies
RUN pip install --no-cache-dir -r /tmp/requirements.txt

# Set the SageMaker program name (this should be your main script)
ENV SAGEMAKER_PROGRAM Main.py

# Copy the model files
COPY awsblogs-vocab.json /opt/ml/model/
COPY awsblogs-merges.txt /opt/ml/model/

# Copy your script(s)
COPY Main.py /opt/ml/code/
