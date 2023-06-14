FROM python:3.8-slim-buster

COPY requirements.txt /tmp/
RUN pip install --no-cache-dir -r /tmp/requirements.txt

COPY awsblogs-vocab.json /opt/ml/model/
COPY awsblogs-merges.txt /opt/ml/model/
COPY index.py /opt/program/

ENTRYPOINT ["python", "/opt/program/index.py"]
